using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using OdinWeb.Models;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.ComponentModel.DataAnnotations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdinWeb.Models.Data.Classes;

namespace OdinWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserModel _userModel;
        private readonly IBranchModel _branchModel;


        public AuthController(IUserModel userModel, IBranchModel branchModel)
        {
            _userModel = userModel;
            _branchModel = branchModel;
        }
        public async Task<IActionResult> Login()
        {

            return View();
        }

        public async Task<IActionResult> Registration()
        {
            List<Branch> comboBranch = _branchModel.GetBranch();
            ViewBag.ComboBranch = comboBranch.Select(t => new SelectListItem
            {
                Value = t.id.ToString(),
                Text = t.name
            }).ToList();

            return View();
        }

        [HttpPost]
        //[SessionState(SessionStateBehavior.Required)]
        public async Task<IActionResult> Validate(LoginViewModel userDTO)
        {

            try
            {
                userDTO.User.password = _userModel.HashPassword(userDTO.User.password);
                var json = JsonConvert.SerializeObject(userDTO.User);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var user = _userModel.Login(userDTO.User);

                if (user != null)
                {
                    var cookieOptions = new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddHours(1), // Establece la expiración de la cookie
                        Secure = true, // Establece la cookie como segura si utilizas HTTPS
                        HttpOnly = true // Evita que el token sea accesible desde JavaScript
                    };
                    Response.Cookies.Append("Token", user.token, cookieOptions);
                    Response.Cookies.Append("Id", user.id.ToString(), cookieOptions);
                    Response.Cookies.Append("IdBranch", user.idBranch.ToString(), cookieOptions);
                    Response.Cookies.Append("Rol", user.rol.name.ToString(), cookieOptions);
                    Response.Cookies.Append("NombreCompleto", user.name + " " + user.lastName, cookieOptions);


                    // Almacena el token en una cookie

                    //HttpContext.Session.SetString("OdinToken", user.token);
                    if (user.restorePass == false)
                    {
                        var rolName = user.rol.name;
                        List<Claim> claims = new List<Claim>()
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.mail),
                                new Claim("id", user.id.ToString()),
                                new Claim(ClaimTypes.Role, rolName)
                            };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        AuthenticationProperties properties = new AuthenticationProperties()
                        {
                            AllowRefresh = true
                        };

                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,

                            new ClaimsPrincipal(claimsIdentity), properties);
                        TempData["AlertMessage"] = "Inicio de Sesión Valido";
                        TempData["AlertType"] = "success";
                        switch (user.rol.name)
                        {
                            case "Admin":
                                return RedirectToAction("Index", "Home");
                            case "Supervisor":
                                return RedirectToAction("Principal", "Supervisor");
                            default:
                                return RedirectToAction("Principal", "Cliente");
                        }
                    }
                    else
                    {
                        ChangePassword changePassword = new ChangePassword();
                        changePassword.id = user.id;
                        TempData["AlertMessage"] = "Se requeire el cambio de contraseña.";
                        TempData["AlertType"] = "info";
                        return RedirectToAction("ChangePassword", changePassword);
                    }

                }


                TempData["AlertMessage"] = "Error, verifique las credenciales.";
                TempData["AlertType"] = "error";

                return RedirectToAction(nameof(Login));
            }
            catch
            {
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegister user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    User newUser = new User();
                    newUser.name = user.name;
                    newUser.lastName = user.lastName;
                    newUser.mail = user.mail;
                    newUser.phone = user.phone;
                    newUser.idBranch = user.idBranch;
                    //Temporal
                    newUser.photo = "./user.png";
                    newUser.password = user.password;
                    newUser.password = _userModel.HashPassword(newUser.password);
                    Rol rol = new Rol();
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync("https://localhost:7271/api/Rol/First"))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                rol = JsonConvert.DeserializeObject<Rol>(apiResponse);
                            }
                        }
                    }

                    newUser.idRol = rol.id;
                    var contexto = new ValidationContext(newUser, serviceProvider: null, items: null);
                    var resultados = new List<ValidationResult>();

                    var isValid = Validator.TryValidateObject(newUser, contexto, resultados, true);

                    if (isValid)
                    {
                        var json = JsonConvert.SerializeObject(newUser);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            using (var response = await httpClient.PostAsync("https://localhost:7271/api/User", data))
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    TempData["AlertMessage"] = "Se creo la cuenta con éxito";
                                    TempData["AlertType"] = "success";
                                    return RedirectToAction("Login", "Auth");
                                }
                            }
                        }

                    }
                }
                TempData["AlertMessage"] = "Error al crear la cuenta";
                TempData["AlertType"] = "error";

                return RedirectToAction(nameof(Registration));
            }
            catch
            {
                return RedirectToAction(nameof(Registration));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestorePassword(LoginViewModel login)
        {


            if (!string.IsNullOrEmpty(login.RestorePassword?.mail) || !string.IsNullOrEmpty(login.RestorePassword?.phone))
            {
                // Ambos campos (mail y phone) tienen valor, puedes continuar con el proceso de restablecimiento de contraseña
                bool resultado = await _userModel.RestorePassword(login.RestorePassword);
                if (resultado)
                {

                    TempData["AlertMessage"] = "Contraseña restablecida con éxito.";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    TempData["AlertMessage"] = "Error a restablecer la contraseña.";
                    TempData["AlertType"] = "error";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                // Al menos uno de los campos (mail o phone) es requerido, muestra un mensaje de error
                ModelState.AddModelError(string.Empty, "Debe proporcionar al menos el correo electrónico o el teléfono para restablecer la contraseña.");
            }
            TempData["mostrarSweetAlert"] = true;

            return RedirectToAction(nameof(Login)); ;

        }
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]

        public async Task<IActionResult> CerrarSesion()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                Response.Cookies.Delete("Token");
            }
            catch (Exception)
            {
            }
            return RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> ChangePassword(ChangePassword user)
        {

            return View(user);

        }
        [HttpPost]
        public IActionResult ChangePasswordP(ChangePassword user)
        {
            if (Request.Cookies["Id"] != null)
            {
                int id = int.Parse(Request.Cookies["Id"]);
                user.id = id;
            }
            try
            {

                var respuesta = _userModel.ChangePassword(user);
                if (respuesta != null)
                {
                    TempData["AlertMessage"] = "Cambio de contraseña con éxito";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(CerrarSesion));

                }
                TempData["AlertMessage"] = "Error, verifique los datos";
                TempData["AlertType"] = "error";
                return RedirectToAction("ChangePassword", user);

            }
            catch
            {

                TempData["AlertMessage"] = "Error, verifique los datos";
                TempData["AlertType"] = "error";
                return RedirectToAction("ChangePassword", new { user = user });

            }

        }
    }
}