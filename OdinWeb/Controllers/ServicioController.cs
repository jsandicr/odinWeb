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
        public async Task<IActionResult> Crear()
        {
            return View();
        }
        public async Task<IActionResult> Ver()
        {
            return View();
        }
        public async Task<IActionResult> Editar()
        {
            return View();
        }
    }
}