using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Data.Interfaces;

namespace OdinWeb.Controllers
{
    public class TransLogController : Controller
    {
        private readonly ITransLogModel _transLogModel;

        public TransLogController(ITransLogModel transLogModel){
            _transLogModel = transLogModel;

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, int> options = new Dictionary<string, int>
            {
                { "7 días", 7 },
                { "15 días", 15 },
                { "30 días", 30 },
                { "60 días", 60 },
                { "90 días", 90 }
            };
            ViewData["DiasOptions"] = options;

            var logs = await _transLogModel.GetAsync();
            return View(logs);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<bool> DeleteTras(string days)
        {
            try
            {
                bool success = await _transLogModel.DeleteAsync(int.Parse(days));

                if (success)
                {
                    return true; // Devuelve true si se eliminó correctamente
                }
                else
                {
                    return false; // Devuelve false si no se pudo eliminar
                }
            }
            catch (Exception)
            {
                return false; // Devuelve false si ocurrió una excepción
            }
        }
    }
}
