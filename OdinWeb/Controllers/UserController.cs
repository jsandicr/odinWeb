using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Controllers
{
    public class UserController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
    }
}
