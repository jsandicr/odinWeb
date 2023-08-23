using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Data.Interfaces;

namespace OdinWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly IReportModel _reportModel;
        public HomeController(IReportModel reportModel)
        {
            _reportModel = reportModel;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var cantAssign = _reportModel.GetCantTicketsAssigned();
            ViewData["CantAssign"] = cantAssign;

            var cantOpen = _reportModel.GetCantTicketsOpen();
            ViewData["CantOpen"] = cantOpen;

            return View();
        }
    }
}
