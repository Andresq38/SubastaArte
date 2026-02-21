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
    public class ObjetoController : Controller
    {
        private readonly IServiceObjeto _serviceObjeto;

        public ObjetoController(IServiceObjeto serviceObjeto)
        {
            _serviceObjeto = serviceObjeto;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceObjeto.ListAsync();
            return View(collection);
        }

        public async Task<ActionResult> IndexAdmin(int? page)
        {
            var collection = await _serviceObjeto.ListAsync();
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
                       "Objeto No encontrado",
                       $"No existe un Objeto sin ID",
                       SweetAlertMessageType.error
                   );
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _serviceObjeto.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Objeto no existente");

                }
                ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
                   "Detalle del Objeto",
                   $"Mostrando información del Objeto: {@object.Nombre}",
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
