using SubastaArte.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SubastaArte.web.Util;

namespace SubastaArte.web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IServiceUsuario _serviceUsuario;

        public UsuarioController(IServiceUsuario serviceUsuario)
        {
            _serviceUsuario = serviceUsuario;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceUsuario.ListAsync();
            return View(collection);
        }

        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {

                    TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                        "Usuario no encontrado",
                        $"No existe un usuario sin ID",
                        SweetAlertMessageType.error
                    );
                    return RedirectToAction("Index");
                }
                var @object = await _serviceUsuario.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Usuario no existente");

                }
                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                    "Detalle del Usuario",
                    $"Mostrando información del Usuario: {@object.Nombre + ' ' + @object.Apellido1 + ' ' + @object.Apellido2}",
                    SweetAlertMessageType.info
                );
                return View(@object);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
