using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OdinWeb.Models;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

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
        public async Task<IActionResult> Guardar(Service service)
        {
            var Folder = Path.Combine(_serviceModel.WebRootPath, "images");
            var FileName = Guid.NewGuid().ToString() + "_" + service.PRD_UPLOAD.FileName;
            var filePath = Path.Combine(Folder, FileName);
            service.photo = service.PRD_UPLOAD.FileName;

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                service.PRD_UPLOAD.CopyTo(fileStream);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var servicio = _serviceModel.PostServicos(service);

                    if (servicio)
                    {
                        TempData["AlertMessage"] = "¡Se creó el servicio!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));

                    }
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el servicio!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el servicio!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Actualizar(Service service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var servicio = _serviceModel.PutServicioById(service);

                    if (servicio)
                    {
                        TempData["AlertMessage"] = "¡Se actualizó el servicio!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));
                    }

                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el servicio!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el servicio!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
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