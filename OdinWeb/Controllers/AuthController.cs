using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models;

namespace OdinWeb.Controllers
{
    public class AuthController : Controller
    {

        public async Task<IActionResult> Login()
        {
            return View();
            //hola
        }
    }
}