using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models;

namespace OdinWeb.Controllers
{
    public class SupervisorController : Controller
    {

        public async Task<IActionResult> Home()
        {
            return View();
        }
    }
}