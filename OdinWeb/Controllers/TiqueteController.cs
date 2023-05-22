using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models;

namespace OdinWeb.Controllers
{
    public class TiqueteController : Controller
    {

        public async Task<IActionResult> Home()
        {
            return View();
        }
    }
}