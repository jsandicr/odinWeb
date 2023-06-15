using Microsoft.AspNetCore.Mvc;

namespace OdinWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
