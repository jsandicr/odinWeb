using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;

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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Guardar(User user, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                user.idRol = 3;
                user.restorePass = true;
                user.password = "password";

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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Actualizar(User user)
        {
            try
            {
                user.idRol = 3;
                user.restorePass = true;
                user.password = _userModel.HashPassword(user.password);
                var servicio = _supervisorModel.PutSupervisorById(user);

                if (servicio)
                {
                    TempData["AlertMessage"] = "¡Se actualizó el Supervisor!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Principal()
        {
            return View();
        }

       
    }
}