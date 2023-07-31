using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;

namespace OdinWeb.Controllers
{
    public class BranchController : Controller
    {

        private readonly IBranchModel _branchModel;

        public BranchController(IBranchModel branchModel)
        {
            _branchModel = branchModel;
        }

        [Authorize]
        public async Task<IActionResult> Home()
        {
            try
            {
                var lista = _branchModel.GetBranch();
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
        public async Task<IActionResult> BranchByTickets()
        {
            try
            {
                var lista = _branchModel.GetBranch();
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
        public async Task<IActionResult> Guardar(Branch branch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var servicio = _branchModel.PostBranch(branch);

                    if (servicio)
                    {
                        TempData["AlertMessage"] = "¡Se creó la sucursal!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));

                    }
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al crear la sucursal!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear la sucursal!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
        }

        [Authorize]
        public async Task<IActionResult> Ver(int id)
        {
            try
            {

                var branch = _branchModel.GetBranchById(id);
                if (branch != null)
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

                var branch = _branchModel.GetBranchById(id);
                if (branch != null)
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
        public async Task<IActionResult> Actualizar(Branch branch)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _branchModel.PutBranchById(branch);

                    if (response)
                    {
                        TempData["AlertMessage"] = "¡Se actualizó la sucursal!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));
                    }

                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar la sucursal!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar la sucursal!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var respuesta = _branchModel.DeleteBranchById(id);

                if (respuesta)
                {
                    TempData["AlertMessage"] = "¡Se eliminó la sucursal!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar la sucursal!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar la sucursal!";
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
