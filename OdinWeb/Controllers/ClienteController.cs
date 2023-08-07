using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OdinWeb.Models;
using OdinWeb.Models.Data.Classes;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Controllers
{
    public class ClienteController : Controller
    {

        private readonly IClienteModel _clientModel;
        private readonly IUserModel _userModel;
        private readonly IBranchModel _branchModel;
        private readonly IChatModel _chatModel;

        public ClienteController(IChatModel chatModel, IClienteModel clientModel, IUserModel userModel, IBranchModel branchModel)
        {
            _clientModel = clientModel;
            _userModel = userModel;
            _branchModel = branchModel;
            _chatModel = chatModel;
        }

        [Authorize]
        public async Task<IActionResult> Home()
        {
            try
            {
                var lista = _clientModel.GetClients();
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
        public async Task<IActionResult> Guardar(User user)
        {
            try
            {
                user.idRol = 1;
                user.restorePass = true;
                user.password = _userModel.HashPassword(user.password);
                if (ModelState.IsValid)
                {
                    var servicio = _clientModel.PostClient(user);

                    if (servicio)
                    {
                        TempData["AlertMessage"] = "¡Se creó el cliente!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));

                    }
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el cliente!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el cliente!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
        }

        [Authorize]
        public async Task<IActionResult> Ver(int id)
        {
            try
            {

                var client = _clientModel.GetClientById(id);
                if (client != null)
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

                    
                    return View(client);
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

                var client = _clientModel.GetClientById(id);
                if (client != null)
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

                    
                    return View(client);
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
                user.idRol = 1;
                user.restorePass = true;
                user.password = _userModel.HashPassword(user.password);
                if (ModelState.IsValid)
                {
                    var servicio = _clientModel.PutClientById(user);

                    if (servicio)
                    {
                        TempData["AlertMessage"] = "¡Se actualizó el cliente!";
                        TempData["AlertType"] = "success";
                        return RedirectToAction(nameof(Home));
                    }

                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el cliente!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el cliente!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var respuesta = _clientModel.DeleteClientById(id);

                if (respuesta)
                {
                    TempData["AlertMessage"] = "¡Se eliminó el cliente!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el cliente!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el cliente!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetAnswer()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Principal()
        {

            try
            {
                var respuesta = _chatModel.GetChat();


                ViewBag.Questions = new List<Chat>();
                foreach (Chat chat in respuesta)
                {
                    ViewBag.Questions.Add(new Chat
                    {
                        Text = chat.Text,
                        Answer = chat.Answer
                    });
                };
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
