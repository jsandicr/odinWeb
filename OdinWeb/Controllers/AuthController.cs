using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using OdinWeb.Models;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OdinWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserModel _userModel;

        public AuthController(IUserModel userModel)
        {
            _userModel = userModel;
        }
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        //[SessionState(SessionStateBehavior.Required)]
        public async Task<IActionResult> Validate(LoginViewModel userDTO)
        {
            
            try
            {             
                    var json = JsonConvert.SerializeObject(userDTO.User);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    User user = new User();
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.PostAsync("https://localhost:7271/api/User/Login", data))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                user = JsonConvert.DeserializeObject<User>(apiResponse);
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

                                // Almacena el token en una cookie
                                var cookieOptions = new CookieOptions
                                {
                                    Expires = DateTime.UtcNow.AddHours(1), // Establece la expiración de la cookie
                                    Secure = true, // Establece la cookie como segura si utilizas HTTPS
                                    HttpOnly = true // Evita que el token sea accesible desde JavaScript
                                };
                                Response.Cookies.Append("Token", user.token, cookieOptions);
                                //HttpContext.Session.SetString("OdinToken", user.token);
                                
                                return RedirectToAction("Home", "Tiquete");
                            }
                        }
                    }
                
                return RedirectToAction(nameof(Login));

            }
            catch
            {
                return RedirectToAction(nameof(Login));
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestorePassword(LoginViewModel login) {

           
                if (!string.IsNullOrEmpty(login.RestorePassword?.mail) || !string.IsNullOrEmpty(login.RestorePassword?.phone))
                {
                    // Ambos campos (mail y phone) tienen valor, puedes continuar con el proceso de restablecimiento de contraseña
                    var resultado = _userModel.RestorePassword(login.RestorePassword);
                    if (resultado == null)
                    {
                        return RedirectToAction(nameof(Login));
                    }
                    else
                    {
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
    }
}