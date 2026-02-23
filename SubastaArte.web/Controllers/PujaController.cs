using Microsoft.AspNetCore.Mvc;

namespace SubastaArte.web.Controllers
{
    public class PujaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
