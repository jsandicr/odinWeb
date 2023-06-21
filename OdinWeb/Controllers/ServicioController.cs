using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net;
using System.Net.Http.Headers;

namespace OdinWeb.Controllers
{
    public class ServicioController : Controller
    {
        private readonly IServicioModel _serviceModel;

        public ServicioController(IServicioModel serviceModel)
        {
            _serviceModel = serviceModel;
        }

        [Authorize]
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

        [Authorize]
        public async Task<IActionResult> Crear()
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
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Guardar(ServiceUDP s, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            var service = new Service();
            service.name = s.name;
            service.description = s.description;
            service.active = s.active;

            var archivoImagen = s.image;

            var nombreArchivo = Path.GetFileName(archivoImagen.FileName);
            var extension = Path.GetExtension(nombreArchivo);
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

        [Authorize]
        public async Task<IActionResult> Ver(int id)
        {
            try
            {

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


                    ServiceUDP s = new ServiceUDP();
                    s.name = servico.name;
                    s.description = servico.description;
                    s.active = servico.active;
                    s.photo = servico.photo;
                    ViewData["Estados"] = estados;
                    return View(s);

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
        public async Task<IActionResult> Actualizar(ServiceUDP s, [FromServices] IWebHostEnvironment hostingEnvironment)
        {

            var service = _serviceModel.GetServicioById(s.id);
            service.name = s.name;
            service.description = s.description;
            service.active = s.active;

            var archivoImagen = s.image;


            if (archivoImagen != null && archivoImagen.Length > 0)
            {
                var nombreArchivo = Path.GetFileName(archivoImagen.FileName);
                var extension = Path.GetExtension(nombreArchivo);
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

        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
               var respuesta = _serviceModel.DeleteServicioById(id);

                if (respuesta)
                {
                    TempData["AlertMessage"] = "¡Se eliminó el servicio!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el servicio!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el servicio!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
        }
    }
}