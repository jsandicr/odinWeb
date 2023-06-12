using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OdinWeb.Models;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Controllers
{
    public class ServicioController : Controller
    {

        [Authorize]
        public async Task<IActionResult> Home()
        {
            try
            {
                List<Service> serviceList = new List<Service>();
                using (var httpClient = new HttpClient())
                {
                    var token = Request.Cookies["Token"];
                    // Agrega el encabezado de autorización con el token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.GetAsync("https://localhost:7271/api/Service"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            serviceList = JsonConvert.DeserializeObject<List<Service>>(apiResponse);
                        }
                    }
                }
                return View(serviceList);
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
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(service);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var token = Request.Cookies["Token"];
                        // Agrega el encabezado de autorización con el token
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.PostAsync("https://localhost:7271/api/Service", data))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["AlertMessage"] = "¡Se creó el servicio!";
                                TempData["AlertType"] = "success";
                                return RedirectToAction(nameof(Home));
                            }
                        }
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
            Service service = new Service();
            using (var httpClient = new HttpClient())
            {
                var token = Request.Cookies["Token"];
                // Agrega el encabezado de autorización con el token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync("https://localhost:7271/api/Service/" + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        service = JsonConvert.DeserializeObject<Service>(apiResponse);
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
            return View(service);
        }

        [Authorize]
        public async Task<IActionResult> Editar(int id)
        {
            Service service = new Service();
            using (var httpClient = new HttpClient())
            {
                var token = Request.Cookies["Token"];
                // Agrega el encabezado de autorización con el token
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var response = await httpClient.GetAsync("https://localhost:7271/api/Service/"+id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        service = JsonConvert.DeserializeObject<Service>(apiResponse);
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
            return View(service);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Actualizar(Service service)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(service);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        var token = Request.Cookies["Token"];
                        // Agrega el encabezado de autorización con el token
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.PutAsync("https://localhost:7271/api/Service/"+service.id, data))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["AlertMessage"] = "¡Se actualizó el servicio!";
                                TempData["AlertType"] = "success";
                                return RedirectToAction(nameof(Home));
                            }
                        }
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
                using (var httpClient = new HttpClient())
                {
                    var token = Request.Cookies["Token"];
                    // Agrega el encabezado de autorización con el token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.DeleteAsync("https://localhost:7271/api/Service/" + id))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["AlertMessage"] = "¡Se eliminó el servicio!";
                            TempData["AlertType"] = "success";
                            return RedirectToAction(nameof(Home));
                        }
                    }
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