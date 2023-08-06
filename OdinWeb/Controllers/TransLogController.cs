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
        public async Task<IActionResult> Index()
        {
            var logs = await _transLogModel.GetAsync();
            return View(logs);
        }
    }
}
