﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Obj;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdinWeb.Models.Data.Interfaces;
using OdinApi.Models.Obj;
using Microsoft.AspNetCore.StaticFiles;

namespace OdinWeb.Controllers
{
    public class TicketController : Controller
    {

        private readonly ITicketModel _ticketModel;
        private readonly IDocumentModel _documentModel;

        private readonly IClienteModel _clientModel;
        private readonly ISupervisorModel _supervisorModel;
        private readonly IServicioModel _serviceModel;
        private readonly IStatusModel _statusModel;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _env;


        public TicketController(
            ITicketModel ticketModel,
            IClienteModel clientModel,
            ISupervisorModel supervisorModel,
            IServicioModel serviceModel,
            IStatusModel statusModel,
            IHttpContextAccessor httpContextAccessor,
            IDocumentModel documentModel,
            IWebHostEnvironment env
        )
        {
            _ticketModel = ticketModel;
            _clientModel = clientModel;
            _supervisorModel = supervisorModel;
            _serviceModel = serviceModel;
            _statusModel = statusModel;
            _httpContextAccessor = httpContextAccessor;
            _documentModel = documentModel;
            _env = env;
        }

        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Crear(int idService)
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

            Ticket ticket = new Ticket();

            var services_ = _serviceModel.GetServicioById(idService);
            TempData["Servicio"] = services_.name;
            ticket.service = services_;
            return View(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Crear(Ticket ticket, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                ticket.creationDate = DateTime.Now;
                ticket.updateDate = DateTime.Now;
                ticket.active = true;
                var documentos = ticket.Archivos;
                ticket.Archivos = null;

                
                var servicio = _ticketModel.PostTicket(ticket);

                if (servicio != null)
                {
                    if (documentos != null)
                    {
                        foreach (var item in documentos)
                        {
                            var nombreArchivo = Path.GetFileName(item.FileName);
                            var extension = Path.GetExtension(nombreArchivo);
                            var nombreUnico = Guid.NewGuid().ToString() + extension;

                            var rutaGuardar = Path.Combine(hostingEnvironment.WebRootPath, "Document", nombreUnico);

                            using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            Documento document = new Documento();
                            document.name = nombreUnico;
                            document.idUser = int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Id"]);
                            document.idTicket = servicio.id;
                            document.nameDocument = nombreArchivo;
                            var drespuesta = _documentModel.PostDocument(document);
                            if (!drespuesta)
                            {
                                TempData["AlertMessage"] = "¡Ocurrio un error al crear el ticket!";
                                TempData["AlertType"] = "error";
                                return View(ticket.idService);
                            }

                        }
                    }

                    TempData["AlertMessage"] = "¡Se creó el ticket Cod-"+ticket.id+"!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(TiquetesProcesoAS));

                    
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el ticket!";
                TempData["AlertType"] = "error";
                return View(ticket.idService);
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el ticket!";
                TempData["AlertType"] = "error";
                return View(ticket.idService);
            }
        }

        [Authorize(Roles = "Admin,Supervisor")]
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
                return RedirectToAction(nameof(TiquetesProcesoAS));
            }
            catch
            {
                return RedirectToAction(nameof(TiquetesProcesoAS));
            }

        }

        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var ticket = _ticketModel.GetTicketById(id);
                if (ticket != null)
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
                            if (status.description != "Finalizado" || Request.Cookies["Rol"] != "Supervisor")
                            {
                                statusOps.Add(new SelectListItem
                                {
                                    Text = status.description,
                                    Value = status.id.ToString()
                                });
                            }

                        };

                        ViewData["Status"] = statusOps;
                    }


                    return View(ticket);
                }
                return RedirectToAction(nameof(TiquetesProcesoAS));
            }
            catch
            {
                return RedirectToAction(nameof(TiquetesProcesoAS));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Actualizar(Ticket ticket)
        {
            try
            {
                ticket.updateDate = DateTime.Now;
                if (ticket.idStatus == 4)
                {
                    ticket.closeDate = DateTime.Now;
                }
                ticket.updateDate= DateTime.Now;
                    var response = _ticketModel.PutTicketById(ticket);

                    if (response)
                    {
                        TempData["AlertMessage"] = "¡Se actualizó el ticket!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(TiquetesProcesoAS));
                    }


                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar), new {id=ticket.id });
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar), new { id = ticket.id });
            }
        }

        [Authorize]
        [Authorize(Roles = "Admin,Supervisor")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var respuesta = _ticketModel.DeleteTicketById(id);

                if (respuesta)
                {
                    TempData["AlertMessage"] = "¡Se eliminó el ticket!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(TiquetesProcesoAS));
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(TiquetesProcesoAS));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el ticket!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(TiquetesProcesoAS));
            }
        }


        [HttpGet]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CrearTiquete(int idService)
        {
            Ticket ticket = new Ticket();
            var services = _serviceModel.GetServicioById(idService);
            TempData["Servicio"] = services.name;
            ticket.service = services;
            return View(ticket);
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]

        public async Task<IActionResult> CrearTiquete(Ticket ticket, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try {

                ticket.idClient = int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Id"]);
                ticket.active = true;
                ticket.creationDate = DateTime.Now;
                ticket.updateDate = DateTime.Now;  
                var service = _serviceModel.GetServicioById(ticket.idService);
                if (service.toAdministrator)
                {
                    ticket.idSupervisor = 1;

                }
                else {
                    var supervisor = _supervisorModel.GetSupervisorSucursal(int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Id"]));
                    ticket.idSupervisor = supervisor.Result.id;
                }       
                ticket.idStatus = 1;
                var documentos = ticket.Archivos;
                ticket.Archivos = null;
                var respuesta = _ticketModel.PostTicket(ticket);
                if (respuesta != null) {
                    if (documentos != null)
                    {
                        foreach (var item in documentos) {
                            var nombreArchivo = Path.GetFileName(item.FileName);
                            var extension = Path.GetExtension(nombreArchivo);
                            var nombreUnico = Guid.NewGuid().ToString() + extension;

                            var rutaGuardar = Path.Combine(hostingEnvironment.WebRootPath, "Document", nombreUnico);

                            using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            Documento document = new Documento();
                            document.name = nombreUnico;
                            document.idUser = int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Id"]);
                            document.idTicket = respuesta.id;
                            document.nameDocument = nombreArchivo;
                            var drespuesta = _documentModel.PostDocument(document);
                            if (!drespuesta) {
                                TempData["AlertMessage"] = "¡Ocurrio un error al crear el ticket!";
                                TempData["AlertType"] = "error";
                                return View(ticket.idService);
                            }

                        }
                    }
                    TempData["AlertMessage"] = "¡Se creó el ticket Cod-"+respuesta.id+"!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(TiquetesProceso));


                }
                return View(ticket.idService);

            } catch {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el ticket!";
                TempData["AlertType"] = "error";
                return View(ticket.idService);
            
            }
            
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Supervisor,Cliente")]
        public IActionResult TiquetesProceso()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Supervisor")]

        public IActionResult TiquetesProcesoAS()
        {
            string rol = Request.Cookies["Rol"];
            if (rol == "Supervisor")
            {
                ViewBag.Layout = "~/Views/Shared/_SupervisorLayout.cshtml";
            }
            return View();
        }

        [HttpGet]
        [Authorize]

        public IActionResult TiquetesProcesoAjax(string status)
        {
            var tickets = _ticketModel.GetTicketsClientsStatus(status);
            return PartialView("_ParcialTable", tickets);

        }

        [HttpGet]
        [Authorize]

        public IActionResult TiquetesAsignadosAjax(string status)
        {
            var tickets = _ticketModel.GetAssignedTickets(status);
            return PartialView("_ParcialTableAS", tickets);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetTicketsByBranchAJAX(int idbranch,string status)
        {
            var tickets = _ticketModel.GetTicketsByBranch(idbranch, status);
            return PartialView("_ParcialTableAS", tickets.Result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetTicketsByBranch(int idbranch,string nombre)
        {
            Branch branch= new Branch();
            branch.id= idbranch;
            branch.name= nombre;
            return View(branch);
        }

        [HttpGet]
        [Authorize]

        public IActionResult TiquetesCerrados()
        {
            if (Request.Cookies["Rol"] == "Cliente") {
                var tickets = _ticketModel.GetTicketsClientsStatus("Finalizado");
                return View(tickets);

            }
            else {
                var tickets = _ticketModel.GetAssignedTickets("Finalizado");
                return View(tickets);

            }
        }


        [HttpGet]
        [Authorize(Roles = "Cliente")]
        public IActionResult VerTiquete(int id)
        {
            return View(_ticketModel.GetTicketById(id));
        }

        [HttpGet]
        [Authorize]
        [Authorize(Roles = "Cliente")]
        public IActionResult EditarTiquete(int id)
        {
            return View(_ticketModel.GetTicketById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> EditarTiquete(Ticket ticket, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                var t = _ticketModel.GetTicketById(ticket.id);
                t.description = ticket.description;
                t.updateDate = DateTime.Now;
                t.idStatus = 1;   
                var documentos = ticket.Archivos;
                ticket.Archivos = null;
                var respuesta = _ticketModel.PutTicketById(t);
                if(respuesta)
                {
                    if (documentos != null)
                    {
                        foreach (var item in documentos)
                        {
                            var nombreArchivo = Path.GetFileName(item.FileName);
                            var extension = Path.GetExtension(nombreArchivo);
                            var nombreUnico = Guid.NewGuid().ToString() + extension;

                            var rutaGuardar = Path.Combine(hostingEnvironment.WebRootPath, "Document", nombreUnico);

                            using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            Documento document = new Documento();
                            document.name = nombreUnico;
                            document.idUser = int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Id"]);
                            document.idTicket = t.id;
                            document.nameDocument = nombreArchivo;
                            var drespuesta = _documentModel.PostDocument(document);
                            if (!drespuesta)
                            {
                                TempData["AlertMessage"] = "¡Ocurrio un error al actulizar el ticket!";
                                TempData["AlertType"] = "error";
                                return View(t);
                            }
                        }
                    }
                    TempData["AlertMessage"] = "¡Se actualizo el ticket!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(TiquetesProceso));
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actulizar el ticket!";
                TempData["AlertType"] = "error";
                return View(t);
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actulizar el ticket!";
                TempData["AlertType"] = "error";
                return View();
            }
            return View();
        }

        [HttpGet]
        [Authorize]

        public IActionResult DownloadDocument(string name)
        {
            var webRootPath = _env.WebRootPath; // Obtener la ruta raíz del servidor
            var documentPath = Path.Combine(webRootPath, "Document", name); // Combinar la ruta raíz con la ruta relativa del documento

            if (!System.IO.File.Exists(documentPath))
            {
                return NotFound();
            }

            string fileName = Path.GetFileName(documentPath);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            FileStream fileStream = new FileStream(documentPath, FileMode.Open, FileAccess.Read);
            return File(fileStream, contentType, fileName);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ViewDocument(string name)
        {
            var webRootPath = _env.WebRootPath; // Obtener la ruta raíz del servidor
            var documentPath = Path.Combine(webRootPath, "Document", name); // Combinar la ruta raíz con la ruta relativa del documento

            if (!System.IO.File.Exists(documentPath))
            {
                return NotFound();
            }

            string fileName = Path.GetFileName(documentPath);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            FileStream fileStream = new FileStream(documentPath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, contentType);
        }

        [Authorize(Roles = "Admin,Supevisor")]
        public JsonResult GetTicketsXTime()
        {
            try
            {
                var tickets = _ticketModel.GetTickets();

                var tiemposTranscurridos = tickets
                .Where(ticket => ticket.closeDate != null)
                .Select(ticket => new
                {
                    TicketId = ticket.id, 
                    TiempoTranscurrido = (int)ticket.closeDate.Value.Subtract(ticket.creationDate).TotalDays
                }).ToList();

                return Json(tiemposTranscurridos);
            }
            catch
            {
                return Json(new List<object>());
            }
        }

        [Authorize(Roles = "Admin,Supevisor")]
        public JsonResult GetTicketsOpen_Close()
        {
            try
            {
                var tickets = _ticketModel.GetTickets();

                var cantidadTiquetesAbiertos = tickets.Count(t => t.closeDate == null);
                var cantidadTiquetesCerrados = tickets.Count(t => t.closeDate != null);

                var resultado = new
                {
                    Abiertos = cantidadTiquetesAbiertos,
                    Cerrados = cantidadTiquetesCerrados
                };

                return Json(resultado);
            }
            catch
            {
                return Json(
                    new
                    {
                        Abiertos = 0,
                        Cerrados = 0
                    }
                );
            }
        }
    }
}