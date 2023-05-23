using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models;

namespace OdinWeb.Controllers
{
    public class ClienteController : Controller
    {

        public async Task<IActionResult> Home()
        {
            return View();
        }

        public async Task<IActionResult> Crear()
        {
            return View();
        }
    }
}
