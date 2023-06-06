using Microsoft.AspNetCore.Mvc;

namespace OdinWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
