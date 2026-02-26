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
    public class PujaController : Controller
    {
        private readonly IServicePuja _servicePuja;

        public PujaController(IServicePuja servicePuja)
        {
            _servicePuja = servicePuja;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IndexPS(int idSubasta, int? page)
        {
            var collection = await _servicePuja.ListSubastaIdAsync(idSubasta);
            //Paginado
            int pageNumber = page ?? 1;
            int pageSize = 5;


            //Cantidad de elementos por página
            return View(collection.ToPagedList(pageNumber, pageSize));
        }
    }
}
