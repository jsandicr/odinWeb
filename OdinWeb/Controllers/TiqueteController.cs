using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Common;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Http;

namespace OdinWeb.Controllers
{
    public class TiqueteController : Controller
    {

        [Authorize]
        public async Task<IActionResult> Home()
        {
            try
            {
                List<Ticket> ticketList = new List<Ticket>();
                using (var httpClient = new HttpClient())
                {
                    var token = Request.Cookies["Token"];
                    // Agrega el encabezado de autorización con el token
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.GetAsync("https://localhost:7271/api/Ticket"))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            ticketList = JsonConvert.DeserializeObject<List<Ticket>>(apiResponse);
                        }
                    }
                }
                return View();
            }
            catch(Exception e)
            {
                return View();
            }
            
        }

        [Authorize]
        public async Task<IActionResult> IndexCliente()
        {

            return View();
        }
    }
}