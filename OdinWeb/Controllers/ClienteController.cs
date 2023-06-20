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

        public ClienteController(IClienteModel clientModel, IUserModel userModel)
        {
            _clientModel = clientModel;
            _userModel = userModel;
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
            List<Branch> branches = new List<Branch>();
            using (var httpClient = new HttpClient())
            {
                var token = Request.Cookies["Token"];
                // Agrega el encabezado de autorización con el token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync("https://localhost:7271/api/Branch"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    branches = JsonConvert.DeserializeObject<List<Branch>>(apiResponse);
                }
            }

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

        [Authorize]
        public async Task<IActionResult> Principal()
        {
            return View();
        }
    }
}
