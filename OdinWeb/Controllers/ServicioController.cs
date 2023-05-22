using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models;

namespace OdinWeb.Controllers
{
    public class ServicioController : Controller
    {

        public async Task<IActionResult> Home()
        {
            return View();
        }
    }
}