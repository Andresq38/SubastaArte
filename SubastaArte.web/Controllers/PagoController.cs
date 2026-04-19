using Microsoft.AspNetCore.Mvc;
using SubastaArte.Application.Services.Interfaces;
using SubastaArte.web.Models;
using SubastaArte.web.Util;
using SubastaArte.web.ViewModels;

namespace SubastaArte.web.Controllers
{
    public class PagoController : Controller
    {
        private readonly IServicePago _servicePago;
        private readonly IServiceResultadoSubasta _serviceResultadoSubasta;
        private readonly IServiceSubasta _serviceSubasta;

        public PagoController(
            IServicePago servicePago,
            IServiceResultadoSubasta serviceResultadoSubasta,
            IServiceSubasta serviceSubasta)
        {
            _servicePago = servicePago;
            _serviceResultadoSubasta = serviceResultadoSubasta;
            _serviceSubasta = serviceSubasta;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? filtro = "pendientes")
        {
            // 1) Backfill robusto:
            //    - toma subastas finalizadas
            //    - intenta registrar resultado (si hay pujas)
            //    - si hay resultado, crea pago pendiente (idempotente)
            var finalizadas = await _serviceSubasta.ListAsync(2);
            foreach (var subasta in finalizadas)
            {
                var resultado = await _serviceResultadoSubasta.FindBySubastaIdAsync(subasta.IdSubasta)
                               ?? await _serviceResultadoSubasta.RegistrarResultadoAsync(subasta.IdSubasta, subasta.FechaCierre);

                if (resultado != null)
                {
                    await _servicePago.RegistrarPagoPendienteAsync(subasta.IdSubasta);
                }
            }

            filtro = (filtro ?? "pendientes").Trim().ToLowerInvariant();
            if (filtro != "todas" && filtro != "pendientes" && filtro != "pagadas")
            {
                filtro = "pendientes";
            }

            var pagos = await _servicePago.ListAsync();
            var resultados = await _serviceResultadoSubasta.ListAsync();

            var query = pagos.AsEnumerable();

            if (filtro == "pendientes")
            {
                query = query.Where(x => string.Equals(x.EstadoPago, "Pendiente", StringComparison.OrdinalIgnoreCase));
            }
            else if (filtro == "pagadas")
            {
                query = query.Where(x => string.Equals(x.EstadoPago, "Confirmado", StringComparison.OrdinalIgnoreCase));
            }

            var model = query
                .Select(p =>
                {
                    var resultado = resultados.FirstOrDefault(r => r.IdSubasta == p.IdSubasta);

                    var nombreGanador = resultado == null
                        ? "No disponible"
                        : $"{resultado.IdUsuarioGanadorNavigation.Nombre} {resultado.IdUsuarioGanadorNavigation.Apellido1} {resultado.IdUsuarioGanadorNavigation.Apellido2}".Trim();

                    return new PagoListadoItemViewModel
                    {
                        IdSubasta = p.IdSubasta,
                        NombreSubasta = p.IdSubastaNavigation?.Nombre ?? $"Subasta #{p.IdSubasta}",
                        NombreGanador = nombreGanador,
                        MontoFinal = resultado?.MontoFinal ?? 0,
                        FechaCierre = resultado?.FechaCierre ?? DateTime.MinValue,
                        EstadoPago = p.EstadoPago
                    };
                })
                .OrderByDescending(x => x.FechaCierre)
                .ToList();

            ViewBag.FiltroActual = filtro;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Confirmar(int idSubasta, string? filtro = "pendientes")
        {
            if (idSubasta <= 0)
            {
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Pago inválido",
                    "No se pudo identificar la subasta del pago.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index), new { filtro });
            }

            try
            {
                await _servicePago.ConfirmarPagoAsync(idSubasta);

                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Pago confirmado",
                    "El pago fue confirmado correctamente.",
                    SweetAlertMessageType.success
                );
            }
            catch (Exception ex)
            {
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Error al confirmar pago",
                    ex.Message,
                    SweetAlertMessageType.error
                );
            }

            return RedirectToAction(nameof(Index), new { filtro });
        }
    }
}