using SubastaArte.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SubastaArte.web.Util;
using SubastaArte.Application.DTOs;

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
       
        public async Task<ActionResult> Edit(int id)
        {
            var dto = await _serviceUsuario.FindByIdAsync(id);
            if (dto == null)
            {
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Usuario no encontrado",
                    $"No existe un usuario con ID {id}",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }

        public async Task<ActionResult> ChangeEstado(int id)
        {
            var dto = await _serviceUsuario.FindByIdAsync(id);
            if (dto == null)
            {
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Usuario no encontrado",
                    $"No existe un usuario con ID {id}",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(Details));
            }

            // Puedes mostrar la vista para elegir el nuevo estado (activo/inactivo)
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UsuarioDTO dto)
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
                return View(dto);
            }

            await _serviceUsuario.UpdateAsync(id, dto);

            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                "Usuario actualizado",
                $"El usuario {dto.Nombre} ha sido modificado exitosamente.",
                SweetAlertMessageType.success
            );
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEstado(int id, int idEstadoUsuario)
        {
            if (idEstadoUsuario <= 0)
            {
                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                    "Error de estado",
                    "Debe seleccionar un estado válido.",
                    SweetAlertMessageType.warning
                );
                // Puedes volver a cargar el usuario para mostrar la vista
                var dto = await _serviceUsuario.FindByIdAsync(id);
                return View(dto);
            }

            await _serviceUsuario.ChangeEstadoAsync(id, idEstadoUsuario);

            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                "Estado actualizado",
                $"El estado del usuario ha sido modificado exitosamente.",
                SweetAlertMessageType.success
            );
            return RedirectToAction(nameof(Details), new { id = id });

        }



    }
}
