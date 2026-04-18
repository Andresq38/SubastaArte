using SubastaArte.Application.Services.Interfaces;
using SubastaArte.Application.Services.Implementations;
using SubastaArte.Application.DTOs;
using SubastaArte.web.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace SubastaArte.web.Controllers
{
    public class SubastaController : Controller
    {
        private readonly IServiceSubasta _serviceSubasta;
        private readonly IServiceObjeto _serviceObjeto;
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServicePuja _servicePuja;

        // Variable lógica interna del usuario actual (simulación para pruebas)
        private const int IdUsuarioActualSimulado = 5;

        public SubastaController(IServiceSubasta serviceSubasta,IServiceObjeto serviceObjeto,IServiceUsuario serviceUsuario,IServicePuja servicePuja)
        {
            _serviceSubasta = serviceSubasta;
            _serviceObjeto = serviceObjeto;
            _serviceUsuario = serviceUsuario;
            _servicePuja = servicePuja;
        }

        [HttpGet]
        public async Task<IActionResult> IndexSA(int? page)
        {
            var collection = await _serviceSubasta.ListAsync(1);
            //Paginado
            int pageNumber = page ?? 1;
            int pageSize = 5;


            //Cantidad de elementos por página
            return View(collection.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> IndexSF(int? page)
        {
            // Obtener subastas con estado 2 (Finalizada) y 4 (Cancelada)
            var subastasFinalizada = await _serviceSubasta.ListAsync(2);
            var subastasCancelada = await _serviceSubasta.ListAsync(4);

            // Combinar ambas colecciones
            var collection = subastasFinalizada.Concat(subastasCancelada).ToList();

            //Paginado
            int pageNumber = page ?? 1;
            int pageSize = 5;

            //Cantidad de elementos por página
            return View(collection.ToPagedList(pageNumber, pageSize));
        }

        public async Task<IActionResult> IndexAdmin(int? page)
        {
            var collection = await _serviceSubasta.ListAsync(3);
            //Paginado
            int pageNumber = page ?? 1;
            int pageSize = 5;


            //Cantidad de elementos por página
            return View(collection.ToPagedList(pageNumber, pageSize));
        }

        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                       "Subasta No encontrada",
                       "No existe una subasta sin ID",
                       SweetAlertMessageType.error
                   );
                    return RedirectToAction("Index");
                }

                var subasta = await _serviceSubasta.FindByIdAsync(id.Value);
                if (subasta == null)
                {
                    throw new Exception("Subasta no existente");
                }

                var historialPujas = await _servicePuja.ListSubastaIdAsync(id.Value);
                var historialOrdenado = historialPujas
                    .OrderByDescending(x => x.FechaHora)
                    .ToList();

                var pujaLider = historialPujas
                    .OrderByDescending(x => x.Monto)
                    .ThenByDescending(x => x.FechaHora)
                    .FirstOrDefault();

                var usuarios = await _serviceUsuario.ListAsync() ?? new List<UsuarioDTO>();

                var usuariosDisponibles = usuarios
                    .Where(u => u.IdRol == 3)          // Compradores
                    .Where(u => u.IdEstadoUsuario == 1) // Activos
                    .Select(u => new
                    {
                        IdUsuario = u.IdUsuario,
                        NombreCompleto = $"{u.Nombre} {u.Apellido1} {u.Apellido2}".Trim()
                    })
                    .OrderBy(u => u.NombreCompleto)
                    .ToList();

                ViewBag.HistorialPujas = historialOrdenado;
                ViewBag.PujaLider = pujaLider;
                ViewBag.ListUsuariosPuja = new SelectList(usuariosDisponibles, "IdUsuario", "NombreCompleto");

                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                   "Detalle de la Subasta",
                   $"Mostrando información de la Subasta: {subasta.Nombre}",
                   SweetAlertMessageType.info
               );

                return View(subasta);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ActionResult> DetailsSf(int? id)
        {
            try
            {
                if (id == null)
                {
                    TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                       "Subasta No encontrada",
                       $"No existe una subasta sin ID",
                       SweetAlertMessageType.error
                   );
                    return RedirectToAction("Index");
                }
                var @object = await _serviceSubasta.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Subasta no existente");

                }
                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                   "Detalle de la Subasta",
                   $"Mostrando información de la Subasta: {@object.Nombre}",
                   SweetAlertMessageType.info
               );
                return View(@object);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        // -------------------------
        // Helpers para combos
        // -------------------------
        private async Task LoadCombosAsync(string? selectedObjetoId = null)
        {
            // Objetos disponibles para subasta 
            var objetos = await _serviceObjeto.ListAsync();

            // Filtrar objetos activos y sin subasta activa
            var objetosDisponibles = objetos
                .Where(o => o.IdEstadoObjeto == 1) 
                .Where(o => !o.Subasta.Any(s => s.IdEstadoSubasta == 1)) 
                .ToList();

            ViewBag.ListObjetos = new SelectList(
                items: objetosDisponibles,
                dataValueField: nameof(ObjetoDTO.IdObjeto),
                dataTextField: nameof(ObjetoDTO.Nombre),
                selectedValue: selectedObjetoId
            );

            // Usuarios vendedores/creadores
            ViewBag.ListUsuarios = await _serviceUsuario.ListAsync();
        }

        // GET: SubastaController/Create
        public async Task<ActionResult> Create()
        {
            await LoadCombosAsync();
            // Cargar el usuario actual (ID 2)
            var usuarioActual = await _serviceUsuario.FindByIdAsync(2);
            ViewBag.UsuarioActual = usuarioActual;
            return View(new SubastaDTO());
        }

        // POST: SubastaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SubastaDTO dto, string[] selectedObjetos)
        {
            selectedObjetos ??= Array.Empty<string>();

            // Validación de objeto seleccionado
            if (selectedObjetos.Length == 0 || string.IsNullOrEmpty(selectedObjetos[0]))
            {
                ModelState.AddModelError("selectedObjetos", "Debe seleccionar un objeto para la subasta.");
            }

            // Validación de nombre de subasta
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                ModelState.AddModelError("Nombre", "El nombre de la subasta es requerido.");

            // Validación de fechas
            if (dto.FechaCierre <= dto.FechaInicio)
                ModelState.AddModelError("FechaCierre", "La fecha de cierre debe ser mayor a la fecha de inicio.");

            if (dto.FechaInicio < DateTime.Now.AddMinutes(-5)) // Tolerancia de 5 minutos
                ModelState.AddModelError("FechaInicio", "La fecha de inicio no puede ser anterior al momento actual.");

            // Validación de precios
            if (dto.PrecioBase <= 0)
                ModelState.AddModelError("PrecioBase", "El precio base debe ser mayor a cero.");

            if (dto.IncrementoMinimo <= 0)
                ModelState.AddModelError("IncrementoMinimo", "El incremento mínimo debe ser mayor a cero.");

            // Validaciones adicionales si se seleccionó un objeto
            if (selectedObjetos.Length > 0 && !string.IsNullOrEmpty(selectedObjetos[0]))
            {
                if (int.TryParse(selectedObjetos[0], out int objetoId))
                {
                    var objeto = await _serviceObjeto.FindByIdAsync(objetoId);
                    if (objeto != null)
                    {
                        // Validar que el objeto esté activo
                        if (objeto.IdEstadoObjeto != 1)
                        {
                            ModelState.AddModelError("selectedObjetos", "El objeto seleccionado debe estar activo.");
                        }

                        // Validar que el objeto no tenga otra subasta activa
                        bool tieneSubastaActiva = objeto.Subasta?.Any(s => s.IdEstadoSubasta == 1) == true;
                        if (tieneSubastaActiva)
                        {
                            ModelState.AddModelError("selectedObjetos", "El objeto seleccionado ya tiene una subasta activa.");
                        }

                        // Asignar automáticamente el vendedor del objeto
                        dto.IdVendedor = objeto.IdVendedor;
                    }
                    else
                    {
                        ModelState.AddModelError("selectedObjetos", "El objeto seleccionado no existe.");
                    }
                }
                else
                {
                    ModelState.AddModelError("selectedObjetos", "El objeto seleccionado no es válido.");
                }
            }

            if (!ModelState.IsValid)
            {
                var errores = string.Join("<br>",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                );

                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                    "Errores de validación",
                    $"El formulario contiene errores:<br>{errores}",
                    SweetAlertMessageType.warning
                );
                await LoadCombosAsync(selectedObjetos.FirstOrDefault());
                var usuarioActual = await _serviceUsuario.FindByIdAsync(2);
                ViewBag.UsuarioActual = usuarioActual;
                return View(dto);
            }

            // Asignar valores automáticos
            dto.IdCreador = 2; // Simula el usuario actual como creador
            dto.IdEstadoSubasta = 3; // Estado inicial: No activa

            await _serviceSubasta.AddAsync(dto, selectedObjetos);

            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
               "Subasta creada correctamente",
               $"La subasta '{dto.Nombre}' fue registrada exitosamente.",
               SweetAlertMessageType.success
           );
            return RedirectToAction(nameof(IndexAdmin));
        }

        // GET: SubastaController/Edit
        public async Task<ActionResult> Edit(int id, string? returnTo = null)
        {
            var dto = await _serviceSubasta.FindByIdAsync(id);
            if (dto == null)
            {
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Subasta no encontrada",
                    $"No existe una subasta con ID {id}",
                    SweetAlertMessageType.error
                );

                // Decidir a dónde regresar en caso de error
                if (returnTo == "IndexSA")
                    return RedirectToAction(nameof(IndexSA));
                else
                    return RedirectToAction(nameof(IndexAdmin));
            }


            ViewBag.ReturnTo = returnTo;

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, SubastaDTO dto, string[] selectedObjetos, string? returnTo = null)
        {
            if (!ModelState.IsValid)
            {
                var errores = string.Join("<br>",
                    ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                );

                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                    "Errores de validación",
                    $"El formulario contiene errores:<br>{errores}",
                    SweetAlertMessageType.warning
                );


                ViewBag.ReturnTo = returnTo;
                return View(dto);
            }

            await _serviceSubasta.UpdateAsync(id, dto, selectedObjetos);

            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                "Subasta actualizada",
                $"La subasta {dto.Nombre} ha sido modificada exitosamente.",
                SweetAlertMessageType.success
            );

            // Decidir a dónde regresar basado en el parámetro
            if (returnTo == "IndexSA")
                return RedirectToAction(nameof(IndexSA));
            else
                return RedirectToAction(nameof(IndexAdmin));
        }


        public async Task<ActionResult> ChangeEstado(int id)
        {
            var dto = await _serviceSubasta.FindByIdAsync(id);
            if (dto == null)
            {
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Subasta no encontrada",
                    $"No existe una subasta con ID {id}",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(IndexAdmin));
            }


            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEstado(int id, int idEstadoSubasta, string? returnTo = null)
        {
            if (idEstadoSubasta <= 0)
            {
                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                    "Error de estado",
                    "Debe seleccionar un estado válido.",
                    SweetAlertMessageType.warning
                );
                var dto = await _serviceSubasta.FindByIdAsync(id);
                return View(dto);
            }

            await _serviceSubasta.ChangeEstadoAsync(id, idEstadoSubasta);

            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                "Estado actualizado",
                $"El estado de la subasta ha sido modificado exitosamente.",
                SweetAlertMessageType.success
            );

            // Decidir a dónde regresar basado en el parámetro
            if (returnTo == "IndexSA")
                return RedirectToAction(nameof(IndexSA));
            else
                return RedirectToAction(nameof(IndexAdmin));
        }
                    
        public class RegistrarPujaRequest
        {
            public int IdSubasta { get; set; }
            public decimal Monto { get; set; }
            public int IdUsuario { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarPuja([FromBody] RegistrarPujaRequest request)
        {
            if (request == null || request.IdSubasta <= 0 || request.Monto <= 0)
            {
                return BadRequest(new
                {
                    ok = false,
                    mensaje = "Datos de puja inválidos."
                });
            }

            if (request.IdUsuario <= 0)
            {
                return BadRequest(new
                {
                    ok = false,
                    mensaje = "Debe seleccionar un usuario para registrar la puja."
                });
            }

            try
            {
                var puja = await _servicePuja.RegistrarPujaAsync(
                    request.IdSubasta,
                    request.Monto,
                    request.IdUsuario
                );

                var nombreUsuarioPuja = $"{puja.IdUsuarioNavigation.Nombre} {puja.IdUsuarioNavigation.Apellido1} {puja.IdUsuarioNavigation.Apellido2}".Trim();

                return Json(new
                {
                    ok = true,
                    mensaje = "Puja registrada correctamente.",
                    puja = new
                    {
                        idPuja = puja.IdPuja,
                        idUsuario = puja.IdUsuario,
                        usuario = nombreUsuarioPuja,
                        monto = puja.Monto,
                        fechaHora = puja.FechaHora.ToString("dd/MM/yyyy HH:mm:ss")
                    }
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    ok = false,
                    mensaje = ex.Message
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ok = false,
                    mensaje = "Ocurrió un error inesperado al registrar la puja."
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> VerificarPujaSuperada(int idSubasta, int idUsuario)
        {
            if (idSubasta <= 0 || idUsuario <= 0)
            {
                return BadRequest(new
                {
                    ok = false,
                    mensaje = "Parámetros inválidos."
                });
            }

            var pujas = await _servicePuja.ListSubastaIdAsync(idSubasta);

            if (pujas == null || !pujas.Any())
            {
                return Json(new
                {
                    ok = true,
                    mostrar = false
                });
            }

            var usuarioTienePuja = pujas.Any(p => p.IdUsuario == idUsuario);
            if (!usuarioTienePuja)
            {
                return Json(new
                {
                    ok = true,
                    mostrar = false
                });
            }

            var pujaLider = pujas
                .OrderByDescending(p => p.Monto)
                .ThenByDescending(p => p.FechaHora)
                .First();

            var pujaMasAltaUsuario = pujas
                .Where(p => p.IdUsuario == idUsuario)
                .OrderByDescending(p => p.Monto)
                .ThenByDescending(p => p.FechaHora)
                .First();

            var fueSuperada = pujaLider.IdUsuario != idUsuario && pujaMasAltaUsuario.Monto < pujaLider.Monto;

            return Json(new
            {
                ok = true,
                mostrar = fueSuperada,
                mensaje = fueSuperada ? "Su puja ha sido superada." : string.Empty
            });
        }

        public class CerrarSubastaRequest
        {
            public int IdSubasta { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CerrarSubastaAutomatico([FromBody] CerrarSubastaRequest request)
        {
            if (request == null || request.IdSubasta <= 0)
            {
                return BadRequest(new
                {
                    ok = false,
                    mensaje = "Datos inválidos para cierre de subasta."
                });
            }

            try
            {
                var subasta = await _serviceSubasta.FindByIdAsync(request.IdSubasta);
                if (subasta == null)
                {
                    return NotFound(new
                    {
                        ok = false,
                        mensaje = "La subasta no existe."
                    });
                }

                // Si ya está finalizada, devolvemos OK idempotente
                if (subasta.IdEstadoSubasta == 2)
                {
                    return Json(new
                    {
                        ok = true,
                        mensaje = "La subasta ya estaba finalizada.",
                        estadoId = 2,
                        estadoNombre = "Finalizada"
                    });
                }

                // Si está cancelada, no se cierra por este flujo
                if (subasta.IdEstadoSubasta == 4)
                {
                    return BadRequest(new
                    {
                        ok = false,
                        mensaje = "La subasta está cancelada y no puede finalizarse por cierre automático."
                    });
                }

                // Asegura que no se cierre antes de tiempo
                if (DateTime.Now < subasta.FechaCierre)
                {
                    return BadRequest(new
                    {
                        ok = false,
                        mensaje = "La subasta aún no llegó a su hora de cierre."
                    });
                }

                // Estado Finalizada = 2
                await _serviceSubasta.ChangeEstadoAsync(request.IdSubasta, 2);

                return Json(new
                {
                    ok = true,
                    mensaje = "La subasta se finalizó automáticamente.",
                    estadoId = 2,
                    estadoNombre = "Finalizada"
                });
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ok = false,
                    mensaje = "Ocurrió un error al finalizar la subasta automáticamente."
                });
            }
        }
    }
}
    