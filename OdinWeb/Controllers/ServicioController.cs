﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;

namespace OdinWeb.Controllers
{
    public class ServicioController : Controller
    {
        private readonly IServicioModel _serviceModel;

        public ServicioController(IServicioModel serviceModel)
        {
            _serviceModel = serviceModel;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Home()
        {
            try
            {
                var lista = _serviceModel.GetServicios();
                if (lista != null)
                {
                    return View(lista);

                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Crear()
        {
            var services = _serviceModel.GetServiciosStatus(true);
            if (services != null)
            {
                List<SelectListItem> servicesOps = new List<SelectListItem>();
                servicesOps.Add(new SelectListItem
                {
                    Text = "",
                    Value = "0"
                });
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Guardar(ServiceUDP s, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            var service = new Service();
            service.name = s.name;
            service.description = s.description;
            service.active = s.active;
            service.transport = s.transport;
            service.toAdministrator = s.toAdministrator;
            
            if (s.idServiceMain == 0)
            {
                s.idServiceMain = null;
            }
            else
            {
                service.idServiceMain = s.idServiceMain;
            }
            service.requirements = s.requirements;

            var archivoImagen = s.image;

            if (archivoImagen != null && archivoImagen.Length > 0)
            {
                var nombreArchivo = Path.GetFileName(archivoImagen.FileName);
                var extension = Path.GetExtension(nombreArchivo).ToLower();
                if (extension == ".png" || extension == ".jpg")
                {
                    if (!string.IsNullOrEmpty(s.photo))
                    {
                        var rutaImagenAnterior = Path.Combine(hostingEnvironment.WebRootPath, "images", "services", s.photo);
                        if (System.IO.File.Exists(rutaImagenAnterior))
                        {
                            System.IO.File.Delete(rutaImagenAnterior);
                        }
                    }
                    var nombreUnico = Guid.NewGuid().ToString() + extension;

                    var rutaGuardar = Path.Combine(hostingEnvironment.WebRootPath, "images", "services", nombreUnico);
                    using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                    {
                        archivoImagen.CopyTo(stream);
                    }

                    service.photo = nombreUnico;

                }
                else
                {
                    TempData["AlertMessage"] = "Error, solo se permiten archivos .png o .jpg";
                    TempData["AlertType"] = "error";
                    return RedirectToAction("Editar");

                }
            }
            var respuesta = _serviceModel.PostServicos(service);
            if (respuesta)
            {
                TempData["AlertMessage"] = "Servicio creado correctamente";
                TempData["AlertType"] = "success";
                return RedirectToAction("Home");
            }
            TempData["AlertMessage"] = "Error verfique los datos";
            TempData["AlertType"] = "error";
            return RedirectToAction("Editar");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Ver(int id)
        {
            try
            {
                var services = _serviceModel.GetServiciosStatus(true);
                if (services != null)
                {
                    List<SelectListItem> servicesOps = new List<SelectListItem>();
                    servicesOps.Add(new SelectListItem
                    {
                        Text = "Seleccione un servicio",
                        Value = "0"
                    });
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

                var servico = _serviceModel.GetServicioById(id);
                if (servico != null)
                {
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
                    return View(servico);
                }
                return RedirectToAction(nameof(Home));
            }
            catch{
                return RedirectToAction(nameof(Home));
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var services = _serviceModel.GetServiciosStatus(true);
                if (services != null)
                {
                    List<SelectListItem> servicesOps = new List<SelectListItem>();
                    servicesOps.Add(new SelectListItem
                    {
                        Text = "Seleccione un servicio",
                        Value = "0"
                    });
                    foreach (Service s in services)
                    {
                        servicesOps.Add(new SelectListItem
                        {
                            Text = s.name,
                            Value = s.id.ToString()
                        });
                    };

                    ViewData["Services"] = servicesOps;
                }

                var service = _serviceModel.GetServicioById(id);
                if (service != null)
                {
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


                    ServiceUDP s = new ServiceUDP();
                    s.name = service.name;
                    s.description = service.description;
                    s.active = service.active;
                    s.photo = service.photo;
                    s.transport = service.transport;
                    s.idServiceMain = service.idServiceMain;
                    s.requirements = service.requirements;
                    s.toAdministrator = service.toAdministrator;
                    
                    ViewData["Estados"] = estados;
                    return View(s);
                }
                return RedirectToAction(nameof(Home));
            }
            catch{
                return RedirectToAction(nameof(Home));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Actualizar(ServiceUDP s, [FromServices] IWebHostEnvironment hostingEnvironment)
        {

            var service = _serviceModel.GetServicioById(s.id);
            service.name = s.name;
            service.description = s.description;
            service.active = s.active;
            service.transport = s.transport;
            service.toAdministrator = s.toAdministrator;
            if (s.idServiceMain == 0)
            {
                service.idServiceMain = null;
            }
            else
            {
                service.idServiceMain = s.idServiceMain;
            }
            service.requirements = s.requirements;

            var archivoImagen = s.imageU;


            if (archivoImagen != null && archivoImagen.Length > 0)
            {
                var nombreArchivo = Path.GetFileName(archivoImagen.FileName);
                var extension = Path.GetExtension(nombreArchivo).ToLower();
                if (extension == ".png" || extension == ".jpg")
                {
                    if (!string.IsNullOrEmpty(s.photo))
                    {
                        var rutaImagenAnterior = Path.Combine(hostingEnvironment.WebRootPath, "images", "services", s.photo);
                        if (System.IO.File.Exists(rutaImagenAnterior))
                        {
                            System.IO.File.Delete(rutaImagenAnterior);
                        }
                    }
                    var nombreUnico = Guid.NewGuid().ToString() + extension;

                    var rutaGuardar = Path.Combine(hostingEnvironment.WebRootPath, "images", "services", nombreUnico);
                    using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                    {
                        archivoImagen.CopyTo(stream);
                    }

                    service.photo = nombreUnico;

                }
                else
                {
                    TempData["AlertMessage"] = "Error, solo se permiten archivos .png o .jpg";
                    TempData["AlertType"] = "error";
                    return RedirectToAction("Editar");

                }
            }
            var respuesta = _serviceModel.PutServicioById(service);
            if (respuesta)
            {
                TempData["AlertMessage"] = "Datos actulizados correctamente";
                TempData["AlertType"] = "success";
                return RedirectToAction("Home");
            }
            TempData["AlertMessage"] = "Error verfique los datos";
            TempData["AlertType"] = "error";
            return RedirectToAction("Editar");

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
               var respuesta = _serviceModel.DeleteServicioById(id);

                if (respuesta)
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin,Supervisor,Cliente")]
        public async Task<IActionResult> HCliente()
        {
         
            return View(_serviceModel.GetServiciosStatus(true));
        }

        [Authorize(Roles = "Admin,Supervisor,Cliente")]
        public async Task<IActionResult> SubService(long id) {
            try {
                
                var respuesta = _serviceModel.GetListSubServicioById(id);

                string rol = Request.Cookies["Rol"];

                if (respuesta != null)
                {
                    return View(respuesta);
                }
                if (rol == "Cliente")
                {
                    return RedirectToAction("CrearTiquete", "Ticket", new { idService = id });
                }
                return RedirectToAction("Crear", "Ticket", new { idService = id });
            }

            catch {
                return RedirectToAction("Crear", "Ticket", new { idService = id });

            }
        }

        [Authorize(Roles = "Admin,Supervisor,Cliente")]
        public JsonResult GetServiceById(int id)
        {
            try
            {
                var respuesta = _serviceModel.GetServicioById(id);
                if (respuesta != null)
                {
                    return Json(respuesta);
                }
                return Json(new Service());
            }
            catch
            {
                return Json(new Service());
            }
        }
    }
}