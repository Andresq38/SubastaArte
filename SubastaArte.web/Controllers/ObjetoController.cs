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




    }
}
