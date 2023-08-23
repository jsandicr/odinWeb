using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Data.Interfaces;

namespace OdinWeb.Controllers
{
    public class ErrorLogController : Controller
    {
        private readonly ITransLogModel _transLogModel;

        public ErrorLogController(ITransLogModel transLogModel)
        {
            _transLogModel = transLogModel;

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var error = await _transLogModel.GetAsyncE();
            return View(error);
        }
    }
}
