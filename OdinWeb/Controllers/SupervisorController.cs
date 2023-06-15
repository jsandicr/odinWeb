using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models;

namespace OdinWeb.Controllers
{
    public class SupervisorController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Home()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Editar()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Ver()
        {
            return View();
        }

        public async Task<IActionResult> Principal()
        {
            return View();
        }
    }
}