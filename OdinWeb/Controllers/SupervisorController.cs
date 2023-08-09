using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using OdinWeb.Models;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;

namespace OdinWeb.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly ISupervisorModel _supervisorModel;
        private readonly IUserModel _userModel;
        public readonly IBranchModel _branchModel;

        public SupervisorController(ISupervisorModel supervisorModel, IUserModel userModel, IBranchModel branchModel)
        {
            _supervisorModel = supervisorModel;
            _userModel = userModel;
            _branchModel = branchModel;
        }

        [Authorize]
        public async Task<IActionResult> Home()
        {
            try
            {
                var lista = _supervisorModel.GetSupervisors();
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
            var branches = _branchModel.GetBranch();            

            List<SelectListItem> branchesOps = new List<SelectListItem>();
            foreach (Branch branch in branches)
            {
                branchesOps.Add(new SelectListItem
                {
                    Text = branch.name,
                    Value = branch.id.ToString()
                });
            };

            ViewData["Branches"] = branchesOps;

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Guardar(User user, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                user.idRol = 3;
                user.restorePass = true;
                user.password = "password";
                var archivoImagen = user.Imagen;

                if (archivoImagen != null && archivoImagen.Length > 0)
                {
                    var nombreArchivo = Path.GetFileName(archivoImagen.FileName);
                    var extension = Path.GetExtension(nombreArchivo);
                    if (extension == ".png" || extension == ".jpg")
                    {
                        if (!string.IsNullOrEmpty(user.photo))
                        {
                            var rutaImagenAnterior = Path.Combine(hostingEnvironment.WebRootPath, "images", "Users", user.photo);
                            if (System.IO.File.Exists(rutaImagenAnterior))
                            {
                                System.IO.File.Delete(rutaImagenAnterior);
                            }
                        }
                        var nombreUnico = Guid.NewGuid().ToString() + extension;

                        var rutaGuardar = Path.Combine(hostingEnvironment.WebRootPath, "images", "Users", nombreUnico);
                        using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                        {
                            archivoImagen.CopyTo(stream);
                        }

                        user.photo = nombreUnico;

                    }
                    else
                    {
                        TempData["AlertMessage"] = "Error, solo se permiten archivos .png o .jpg";
                        TempData["AlertType"] = "error";
                        return RedirectToAction("Editar");

                    }
                }


                //if (ModelState.IsValid)
                //{

                //}

                var servicio = _supervisorModel.PostSupervisor(user);

                if (servicio)
                {
                    TempData["AlertMessage"] = "¡Se creó el Supervisor!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));

                }
                else { 
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el Supervisor!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
                }
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el Supervisor!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
        }

        [Authorize]
        public async Task<IActionResult> Ver(int id)
        {
            try
            {

                var Supervisor = _supervisorModel.GetSupervisorById(id);
                if (Supervisor != null)
                {
                    var branches = _branchModel.GetBranch();


                    List<SelectListItem> branchesOps = new List<SelectListItem>();
                    foreach (Branch branch in branches)
                    {
                        branchesOps.Add(new SelectListItem
                        {
                            Text = branch.name,
                            Value = branch.id.ToString()
                        });
                    };

                    ViewData["Branches"] = branchesOps;

                  
                    return View(Supervisor);
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

                var Supervisor = _supervisorModel.GetSupervisorById(id);
                if (Supervisor != null)
                {
                    var branches = _branchModel.GetBranch();


                    List<SelectListItem> branchesOps = new List<SelectListItem>();
                    foreach (Branch branch in branches)
                    {
                        branchesOps.Add(new SelectListItem
                        {
                            Text = branch.name,
                            Value = branch.id.ToString()
                        });
                    };

                    ViewData["Branches"] = branchesOps;

                    return View(Supervisor);
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
        public async Task<IActionResult> Actualizar(User user)
        {
            try
            {
                user.idRol = 3;
                user.restorePass = true;
                user.password = _userModel.HashPassword(user.password);
                if (ModelState.IsValid)
                {
                    var servicio = _supervisorModel.PutSupervisorById(user);

                    if (servicio)
                    {
                        TempData["AlertMessage"] = "¡Se actualizó el Supervisor!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));
                    }

                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el Supervisor!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el Supervisor!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var respuesta = _supervisorModel.DeleteSupervisorById(id);

                if (respuesta)
                {
                    TempData["AlertMessage"] = "¡Se eliminó el Supervisor!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el Supervisor!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el Supervisor!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
        }

        [Authorize]
        public async Task<IActionResult> Principal()
        {
            return View();
        }

       
    }
}