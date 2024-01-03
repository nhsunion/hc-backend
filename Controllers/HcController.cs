using Microsoft.AspNetCore.Mvc;

namespace hc_backend.Controllers
{
    public class HcController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
