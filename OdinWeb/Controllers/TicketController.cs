using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Obj;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdinWeb.Models.Data.Interfaces;
using System;

namespace OdinWeb.Controllers
{
    public class TicketController : Controller
    {

        private readonly ITicketModel _ticketModel;
        private readonly IClienteModel _clientModel;
        private readonly ISupervisorModel _supervisorModel;
        private readonly IServicioModel _serviceModel;
        private readonly IStatusModel _statusModel;

        public TicketController(
            ITicketModel ticketModel,
            IClienteModel clientModel,
            ISupervisorModel supervisorModel,
            IServicioModel serviceModel,
            IStatusModel statusModel
        )
        {
            _ticketModel = ticketModel;
            _clientModel = clientModel;
            _supervisorModel = supervisorModel;
            _serviceModel = serviceModel;
            _statusModel = statusModel;
        }

        [Authorize]
        public async Task<IActionResult> Home(string? filtro)
        {
            try
            {
                if(filtro == null)
                {
                    var lista = _ticketModel.GetTickets();
                    if (lista != null)
                    {
                        return View(lista);
                    }
                    else
                    {
                        return View();
                    }
                }
                if(filtro == "Assigned")
                {
                    if (Request.Cookies.ContainsKey("Id"))
                    {
                        var id = Request.Cookies["Id"];
                        var lista = _ticketModel.GetAssignedTickets(id);
                        if (lista != null)
                        {
                            return View(lista);

                        }
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Crear()
        {
            var clients = _clientModel.GetClients();
            if (clients != null)
            {
                List<SelectListItem> clientsOps = new List<SelectListItem>();
                foreach (User client in clients)
                {
                    clientsOps.Add(new SelectListItem
                    {
                        Text = client.name + " " + client.lastName,
                        Value = client.id.ToString()
                    });
                };

                ViewData["Clients"] = clientsOps;
            }

            var supervisors = _supervisorModel.GetSupervisors();
            if (supervisors != null)
            {
                List<SelectListItem> supervisorsOps = new List<SelectListItem>();
                foreach (User supervisor in supervisors)
                {
                    supervisorsOps.Add(new SelectListItem
                    {
                        Text = supervisor.name + " " + supervisor.lastName,
                        Value = supervisor.id.ToString()
                    });
                };

                ViewData["Supervisors"] = supervisorsOps;
            }

            var services = _serviceModel.GetServicios();
            if (services != null)
            {
                List<SelectListItem> servicesOps = new List<SelectListItem>();
                foreach (Service service in services)
                {
                    servicesOps.Add(new SelectListItem
                    {
                        Text = service.name,
                        Value = service.id.ToString()
                    });
                };

                ViewData["Services"] = servicesOps;
            }

            var statusL = _statusModel.GetStatus();
            if (statusL != null)
            {
                List<SelectListItem> statusOps = new List<SelectListItem>();
                foreach (Status status in statusL)
                {
                    statusOps.Add(new SelectListItem
                    {
                        Text = status.description,
                        Value = status.id.ToString()
                    });
                };

                ViewData["Status"] = statusOps;
            }

            List<SelectListItem> estados = new List<SelectListItem>();
            estados.Add(new SelectListItem
            {
                Text = "Activo",
                Value = "true"
            });

            estados.Add(new SelectListItem
            {
                Text = "Inactivo",
                Value = "false"
            });

            ViewData["Estados"] = estados;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Guardar(Ticket ticket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var servicio = _ticketModel.PostTicket(ticket);

                    if (servicio)
                    {
                        TempData["AlertMessage"] = "¡Se creó el ticket!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));

                    }
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
        }

        [Authorize]
        public async Task<IActionResult> Ver(int id)
        {
            try
            {


                var branch = _ticketModel.GetTicketById(id);
                if (branch != null)
                {
                    var clients = _clientModel.GetClients();
                    if (clients != null)
                    {
                        List<SelectListItem> clientsOps = new List<SelectListItem>();
                        foreach (User client in clients)
                        {
                            clientsOps.Add(new SelectListItem
                            {
                                Text = client.name+" "+client.lastName,
                                Value = client.id.ToString()
                            });
                        };

                        ViewData["Clients"] = clientsOps;
                    }

                    var supervisors = _supervisorModel.GetSupervisors();
                    if (supervisors != null)
                    {
                        List<SelectListItem> supervisorsOps = new List<SelectListItem>();
                        foreach (User supervisor in supervisors)
                        {
                            supervisorsOps.Add(new SelectListItem
                            {
                                Text = supervisor.name + " " + supervisor.lastName,
                                Value = supervisor.id.ToString()
                            });
                        };

                        ViewData["Supervisors"] = supervisorsOps;
                    }

                    var services = _serviceModel.GetServicios();
                    if (services != null)
                    {
                        List<SelectListItem> servicesOps = new List<SelectListItem>();
                        foreach (Service service in services)
                        {
                            servicesOps.Add(new SelectListItem
                            {
                                Text = service.name,
                                Value = service.id.ToString()
                            });
                        };

                        ViewData["Services"] = servicesOps;
                    }

                    var statusL = _statusModel.GetStatus();
                    if (statusL != null)
                    {
                        List<SelectListItem> statusOps = new List<SelectListItem>();
                        foreach (Status status in statusL)
                        {
                            statusOps.Add(new SelectListItem
                            {
                                Text = status.description,
                                Value = status.id.ToString()
                            });
                        };

                        ViewData["Status"] = statusOps;
                    }

                    List<SelectListItem> estados = new List<SelectListItem>();
                    estados.Add(new SelectListItem
                    {
                        Text = "Activo",
                        Value = "true"
                    });

                    estados.Add(new SelectListItem
                    {
                        Text = "Inactivo",
                        Value = "false"
                    });

                    ViewData["Estados"] = estados;
                    return View(branch);
                }
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                return RedirectToAction(nameof(Home));
            }

        }

        [Authorize]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {

                var branch = _ticketModel.GetTicketById(id);
                if (branch != null)
                {
                    var clients = _clientModel.GetClients();
                    if (clients != null)
                    {
                        List<SelectListItem> clientsOps = new List<SelectListItem>();
                        foreach (User client in clients)
                        {
                            clientsOps.Add(new SelectListItem
                            {
                                Text = client.name + " " + client.lastName,
                                Value = client.id.ToString()
                            });
                        };

                        ViewData["Clients"] = clientsOps;
                    }

                    var supervisors = _supervisorModel.GetSupervisors();
                    if (supervisors != null)
                    {
                        List<SelectListItem> supervisorsOps = new List<SelectListItem>();
                        foreach (User supervisor in supervisors)
                        {
                            supervisorsOps.Add(new SelectListItem
                            {
                                Text = supervisor.name + " " + supervisor.lastName,
                                Value = supervisor.id.ToString()
                            });
                        };

                        ViewData["Supervisors"] = supervisorsOps;
                    }

                    var services = _serviceModel.GetServicios();
                    if (services != null)
                    {
                        List<SelectListItem> servicesOps = new List<SelectListItem>();
                        foreach (Service service in services)
                        {
                            servicesOps.Add(new SelectListItem
                            {
                                Text = service.name,
                                Value = service.id.ToString()
                            });
                        };

                        ViewData["Services"] = servicesOps;
                    }

                    var statusL = _statusModel.GetStatus();
                    if (statusL != null)
                    {
                        List<SelectListItem> statusOps = new List<SelectListItem>();
                        foreach (Status status in statusL)
                        {
                            statusOps.Add(new SelectListItem
                            {
                                Text = status.description,
                                Value = status.id.ToString()
                            });
                        };

                        ViewData["Status"] = statusOps;
                    }

                    List<SelectListItem> estados = new List<SelectListItem>();
                    estados.Add(new SelectListItem
                    {
                        Text = "Activo",
                        Value = "true"
                    });

                    estados.Add(new SelectListItem
                    {
                        Text = "Inactivo",
                        Value = "false"
                    });

                    ViewData["Estados"] = estados;
                    return View(branch);
                }
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                return RedirectToAction(nameof(Home));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Actualizar(Ticket ticket)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _ticketModel.PutTicketById(ticket);

                    if (response)
                    {
                        TempData["AlertMessage"] = "¡Se actualizó el ticket!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));
                    }

                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var respuesta = _ticketModel.DeleteTicketById(id);

                if (respuesta)
                {
                    TempData["AlertMessage"] = "¡Se eliminó el ticket!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
        }

        [Authorize]
        public async Task<IActionResult> IndexCliente()
        {
            return View();
        }
    }
}