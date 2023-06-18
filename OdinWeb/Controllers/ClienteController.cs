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
    public class ClienteController : Controller
    {

        private readonly IUserModel _userModel;

        public ClienteController(IUserModel userModel)
        {
            _userModel = userModel;
        }

        [Authorize]
        public async Task<IActionResult> Home()
        {
            try
            {
                List<User> userList = new List<User>();
                using (var httpClient = new HttpClient())
                {
                    var token = Request.Cookies["Token"];
                    // Agrega el encabezado de autorización con el token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.GetAsync("https://localhost:7271/api/User/Client"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            userList = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                        }
                    }
                }
                return View(userList);
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
                user.password = _userModel.HashPassword(user.password);
                //Temporal
                user.photo = "./";
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(user);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var token = Request.Cookies["Token"];
                        // Agrega el encabezado de autorización con el token
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.PostAsync("https://localhost:7271/api/User", data))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["AlertMessage"] = "¡Se creó el usuario!";
                                TempData["AlertType"] = "success";
                                return RedirectToAction(nameof(Home));
                            }
                        }
                    }
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el usuario!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el usuario!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
        }

        [Authorize]
        public async Task<IActionResult> Ver(int id)
        {
            User user = new User();
            using (var httpClient = new HttpClient())
            {
                var token = Request.Cookies["Token"];
                // Agrega el encabezado de autorización con el token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync("https://localhost:7271/api/User/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        user = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                }
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
            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> Editar(int id)
        {
            User user = new User();
            using (var httpClient = new HttpClient())
            {
                var token = Request.Cookies["Token"];
                // Agrega el encabezado de autorización con el token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync("https://localhost:7271/api/User/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        user = JsonConvert.DeserializeObject<User>(apiResponse);
                    }
                }
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
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Actualizar(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(user);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var token = Request.Cookies["Token"];
                        // Agrega el encabezado de autorización con el token
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.PutAsync("https://localhost:7271/api/User/" + user.id, data))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["AlertMessage"] = "¡Se actualizó el usuario!";
                                TempData["AlertType"] = "success";
                                return RedirectToAction(nameof(Home));
                            }
                        }
                    }
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el usuario!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el usuario!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
        }

        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var token = Request.Cookies["Token"];
                    // Agrega el encabezado de autorización con el token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.DeleteAsync("https://localhost:7271/api/User/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["AlertMessage"] = "¡Se eliminó el usuario!";
                            TempData["AlertType"] = "success";
                            return RedirectToAction(nameof(Home));
                        }
                    }
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el usuario!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el usuario!";
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
