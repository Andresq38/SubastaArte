using SubastaArte.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
    }
}
