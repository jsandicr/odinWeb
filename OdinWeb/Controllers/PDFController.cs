using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using Rotativa.AspNetCore;
using System.Globalization;

namespace OdinWeb.Controllers
{
    public class PDFController : Controller
    {
        private readonly IReportModel _reportModel;

        public PDFController(IReportModel reportModel)
        {
            _reportModel = reportModel;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TicketsXTime()
        {
            var tickets = _reportModel.GetTicketsXTime();
            return new ViewAsPdf("TicketsXTime", tickets)
            {

            };
        }

        public IActionResult TicketsXSupervisor()
        {
            var tickets = _reportModel.GetTicketsXSupervisor();

            var cantidadTicketsPorSupervisor = tickets
                .GroupBy(t => t.idSupervisor)
                .Select(g => new { SupervisorName = g.First().supervisor.name + " " + g.First().supervisor.lastName, CantidadTickets = g.Count() })
                .ToList();

            var viewModel = new TicketsXSupervisorViewModel
            {
                CantidadTicketsPorSupervisor = cantidadTicketsPorSupervisor
            };

            return new ViewAsPdf("TicketsXSupervisor", viewModel)
            {

            };
        }

        public IActionResult TicketsXSupervisorM()
        {
            var tickets = _reportModel.GetTicketsXSupervisor();

            var cantidadTicketsPorSupervisorPorMes = tickets
                .GroupBy(t => new { t.idSupervisor, t.creationDate.Month })
                .Select(g => new
                {
                    SupervisorName = g.First().supervisor.name + " " + g.First().supervisor.lastName,
                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                    CantidadTickets = g.Count()
                })
                .ToList();

            var viewModel = new TicketsXSupervisorViewModel
            {
                CantidadTicketsPorSupervisor = cantidadTicketsPorSupervisorPorMes
            };

            return new ViewAsPdf("TicketsXSupervisorM", viewModel)
            {

            };
        }
    }
}