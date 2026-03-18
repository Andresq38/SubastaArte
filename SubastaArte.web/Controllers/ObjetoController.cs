using SubastaArte.Application.Services.Interfaces;
using SubastaArte.Application.Services.Implementations;
using SubastaArte.Application.DTOs;
using SubastaArte.web.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;
using SubastaArte.Infraestructure.Models;


namespace SubastaArte.web.Controllers
{
    public class ObjetoController : Controller
    {
        private readonly IServiceObjeto _serviceObjeto;
        private readonly IServiceUsuario _serviceUsuario;
        private readonly IServiceCategoria _serviceCategoria;

        public ObjetoController(IServiceObjeto serviceObjeto, IServiceUsuario serviceUsuario, IServiceCategoria serviceCategoria)
        {
            _serviceObjeto = serviceObjeto;
            _serviceUsuario = serviceUsuario;
            _serviceCategoria = serviceCategoria;
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

        public async Task<ActionResult> SubastasObjetos(int IdObjeto, int? page)
        {
            var objeto = await _serviceObjeto.FindByIdAsync(IdObjeto);
            if (objeto == null)
            {
                return NotFound();
            }

            var subastas = objeto.Subasta ?? new List<SubastaDTO>();
            int pageNumber = page ?? 1;
            int pageSize = 5;

            return View(subastas.ToPagedList(pageNumber, pageSize));
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

        // -------------------------
        // Helpers para combos
        // -------------------------
        private async Task LoadCombosAsync(IEnumerable<string>? selectedCategoriaIds = null)
        {
            // Autores
            ViewBag.ListAutor = await _serviceUsuario.ListAsync();

            // Categorías (many-to-many)
            var categorias = await _serviceCategoria.ListAsync();

            ViewBag.ListCategorias = new MultiSelectList(
                items: categorias,
                dataValueField: nameof(CategoriaDTO.IdCategoria),
                dataTextField: nameof(CategoriaDTO.Nombre),
                selectedValues: selectedCategoriaIds
            );
        }

        // GET: ObjetoController/Create
        public async Task<ActionResult> Create()
        {
            await LoadCombosAsync();
            // Cargar el usuario vendedor (ID 2)
            var vendedor = await _serviceUsuario.FindByIdAsync(2);
            ViewBag.Vendedor = vendedor;
            return View(new ObjetoDTO());
        }

        // POST: ObjetoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ObjetoDTO dto, List<IFormFile> imagenes, string[] selectedCategorias)
        {
            selectedCategorias ??= Array.Empty<string>();

            // Validación de categorías 
            if (selectedCategorias.Length == 0)
            {
                ModelState.AddModelError("selectedCategorias", "Debe seleccionar al menos una categoría.");
            }

            // Validación de imágenes (mínimo 1)
            if ((dto.Foto == null || dto.Foto.Count == 0) && (imagenes == null || imagenes.Count == 0))
            {
                ModelState.AddModelError("Foto", "Debe seleccionar al menos una imagen.");
            }

            // Procesar imágenes subidas
            if (imagenes != null && imagenes.Count > 0)
            {
                dto.Foto = new List<ImagenDTO>();
                foreach (var img in imagenes)
                {
                    if (img.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await img.CopyToAsync(ms);
                        dto.Foto.Add(new ImagenDTO { Foto = ms.ToArray() });
                    }
                }
                ModelState.Remove("Foto");
            }

            // Validación de nombre y descripción
            if (string.IsNullOrWhiteSpace(dto.Nombre))
                ModelState.AddModelError("Nombre", "El nombre es requerido.");
            if (string.IsNullOrWhiteSpace(dto.Descripcion) || dto.Descripcion.Length < 20)
                ModelState.AddModelError("Descripcion", "La descripción debe tener al menos 20 caracteres.");

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
                await LoadCombosAsync(selectedCategorias);
                var vendedor = await _serviceUsuario.FindByIdAsync(2);
                ViewBag.Vendedor = vendedor;
                return View(dto);
            }

            // Asignar usuario vendedor simulado y estado activo
            dto.IdVendedor = 2; // Simula el usuario actual
            dto.IdEstadoObjeto = 1; // Estado inicial activo (ajusta según tu catálogo)

            await _serviceObjeto.AddAsync(dto, selectedCategorias);

            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
               "Objeto creado correctamente",
               $"El objeto {dto.Nombre} fue registrado exitosamente.",
               SweetAlertMessageType.success
           );
            return RedirectToAction(nameof(IndexAdmin));
        }

        // GET: ObjetoController/Edit/
        public async Task<ActionResult> Edit(int id)
        {
            var dto = await _serviceObjeto.FindByIdAsync(id);

            bool tieneSubastaActiva = dto.Subasta?.Any(s => s.IdEstadoSubasta == 1) == true;
            if (tieneSubastaActiva)
            {
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Edición no permitida",
                    "No se puede editar el objeto porque pertenece a una subasta activa.",
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(IndexAdmin));
            }

            var selected = dto.IdCategoria
                .Select(c => c.IdCategoria.ToString())
                .ToList();

            await LoadCombosAsync(selected);

            var vendedor = await _serviceUsuario.FindByIdAsync(dto.IdVendedor);
            ViewBag.Vendedor = vendedor;

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            int id,
            ObjetoDTO dto,
            List<IFormFile> imagenes,
            string[] selectedCategorias,
            int[] ImagenesConservar)
        {

            selectedCategorias ??= Array.Empty<string>();

            if (selectedCategorias.Length == 0)
            {
                ModelState.AddModelError("selectedCategorias", "Debe seleccionar al menos una categoría.");
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                ModelState.AddModelError("Nombre", "El nombre es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Descripcion) || dto.Descripcion.Length < 20)
                ModelState.AddModelError("Descripcion", "La descripción debe tener al menos 20 caracteres.");

            // ==========================================
            // CONSTRUIR LISTA FINAL DE IMÁGENES
            // ==========================================

            // Construir la lista final de imágenes
            dto.Foto = new List<ImagenDTO>();

            // Imagenes existentes
            if (ImagenesConservar != null && ImagenesConservar.Length > 0)
            {
                foreach (var idImg in ImagenesConservar)
                {
                    dto.Foto.Add(new ImagenDTO { IdImagen = idImg });
                }
            }

            // Imagenes nuevas
            if (imagenes != null && imagenes.Count > 0)
            {
                foreach (var img in imagenes)
                {
                    if (img.Length > 0)
                    {
                        using var ms = new MemoryStream();
                        await img.CopyToAsync(ms);
                        dto.Foto.Add(new ImagenDTO { Foto = ms.ToArray() });
                    }
                }
            }


            // Limpiar validación de Foto
            ModelState.Remove(nameof(dto.Foto));
            ModelState.Remove("Foto");

            // ==========================================

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

                await LoadCombosAsync(selectedCategorias);

                var vendedor = await _serviceUsuario.FindByIdAsync(dto.IdVendedor);
                ViewBag.Vendedor = vendedor;

                return View(dto);
            }

            await _serviceObjeto.UpdateAsync(id, dto, selectedCategorias);

            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                "Objeto actualizado",
                $"El objeto {dto.Nombre} ha sido modificado exitosamente.",
                SweetAlertMessageType.success
            );

            return RedirectToAction(nameof(IndexAdmin));
        }

        // GET: ObjetoController/Delete
        //public async Task<ActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        var dto = await _serviceObjeto.FindByIdAsync(id);
        //        if (dto == null)
        //        {
        //            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
        //                "Objeto no encontrado",
        //                $"No existe un objeto con ID {id}",
        //                SweetAlertMessageType.error
        //            );
        //            return RedirectToAction(nameof(IndexAdmin));
        //        }

        //        // Verificar si el objeto pertenece a subastas activas o finalizadas
        //        bool perteneceASubastaActivaOFinalizada = dto.Subasta?.Any(s => s.IdEstadoSubasta == 1 || s.IdEstadoSubasta == 2) == true;
        //        if (perteneceASubastaActivaOFinalizada)
        //        {
        //            TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
        //                "Eliminación no permitida",
        //                "No se puede eliminar el objeto porque pertenece a una subasta activa o finalizada.",
        //                SweetAlertMessageType.error
        //            );
        //            return RedirectToAction(nameof(IndexAdmin));
        //        }

        //        ViewBag.Notificacion = SweetAlertHelper.CrearNotificacion(
        //            "Confirmar eliminación",
        //            $"¿Está seguro que desea eliminar el objeto '{dto.Nombre}'? Esta acción no se puede deshacer.",
        //            SweetAlertMessageType.warning
        //        );

        //        return View(dto);
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
        //            "Error",
        //            $"Ocurrió un error al intentar acceder al objeto: {ex.Message}",
        //            SweetAlertMessageType.error
        //        );
        //        return RedirectToAction(nameof(IndexAdmin));
        //    }
        //}

        // POST: ObjetoController/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _serviceObjeto.DeleteAsync(id);

                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Objeto eliminado correctamente",
                    "El objeto y sus imágenes han sido eliminados exitosamente.",
                    SweetAlertMessageType.success
                );

                return RedirectToAction(nameof(IndexAdmin));
            }
            catch (InvalidOperationException ex)
            {
                // Este tipo de excepción se lanza cuando hay validaciones de negocio
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "No se puede eliminar",
                    ex.Message,
                    SweetAlertMessageType.error
                );
                return RedirectToAction(nameof(IndexAdmin));
            }
            catch (Exception ex)
            {
                // Para cualquier otro error inesperado
                TempData["Notificacion"] = SweetAlertHelper.CrearNotificacion(
                    "Error al eliminar",
                    $"Ocurrió un error inesperado: {ex.Message}",
                    SweetAlertMessageType.error
                );

                return RedirectToAction(nameof(IndexAdmin));
            }
        }



    }
}
