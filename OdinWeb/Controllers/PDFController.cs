using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using Rotativa.AspNetCore;
using System.Data;
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

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult TicketsXTime(DateTime date1, DateTime date2)
        {
            try
            {
                var tickets = _reportModel.GetTicketsXTime(date1, date2);
                var viewModel = new TicketsXSupervisorViewModel
                {
                    tickets = tickets,
                    desde = date1.ToString("dd-MM-yyyy"),
                    hasta = date2.ToString("dd-MM-yyyy")
                };
                return View(viewModel);
                //return new ViewAsPdf("TicketsXTime", viewModel)
                //{

                //};
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult TicketsXSupervisor(DateTime date1, DateTime date2)
        {
            try
            {
                var tickets = _reportModel.GetTicketsXSupervisor(date1,date2);

                var cantidadTicketsPorSupervisor = tickets
                    .GroupBy(t => t.idSupervisor)
                    .Select(g => new { SupervisorName = g.First().supervisor.name + " " + g.First().supervisor.lastName, CantidadTickets = g.Count() })
                    .ToList();

                var viewModel = new TicketsXSupervisorViewModel
                {
                    CantidadTicketsPorSupervisor = cantidadTicketsPorSupervisor,
                    desde = date1.ToString("dd-MM-yyyy"),
                    hasta = date2.ToString("dd-MM-yyyy")
                };

                return View(viewModel);

                /*return new ViewAsPdf("TicketsXSupervisor", viewModel)
                {

                };*/
            }catch(Exception e)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult TicketsXSupervisorM()
        {
            try
            {
                var tickets = _reportModel.GetTicketsXSupervisorM();

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

                return View(viewModel);
                //return new ViewAsPdf("TicketsXSupervisorM", viewModel)
                //{

                //};
            }
            catch(Exception e)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}