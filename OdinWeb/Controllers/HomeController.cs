using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Data.Classes;
using OdinWeb.Models.Data.Interfaces;

namespace OdinWeb.Controllers
{
    public class HomeController : Controller
    {

        private readonly IReportModel _reportModel;
        private readonly ITicketModel _ticketModel;
        public HomeController(IReportModel reportModel, ITicketModel ticketModel)
        {
            _reportModel = reportModel;
            _ticketModel = ticketModel;
        }

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
