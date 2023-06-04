using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using OdinWeb.Models;
using OdinWeb.Models.Obj;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OdinWeb.Controllers
{
    public class AuthController : Controller
    {

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        //[SessionState(SessionStateBehavior.Required)]
        public async Task<IActionResult> Validate(UserDTO userDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(userDTO);
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
                }
                return RedirectToAction(nameof(Login));

            }
            catch
            {
                return RedirectToAction(nameof(Login));
            }
        }
    }
}