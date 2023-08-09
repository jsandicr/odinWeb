using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class ReportModel : IReportModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportModel(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public List<Ticket> GetTicketsXTime()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = _httpClient.GetAsync("api/Report/TicketsXTime").Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }

        public List<Ticket> GetTicketsXSupervisor()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = _httpClient.GetAsync("api/Report/TicketsXSupervisor").Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }

        public int GetCantTicketsAssigned()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var id = _httpContextAccessor.HttpContext.Request.Cookies["Id"];
            var response = _httpClient.GetAsync("api/Report/CantTicketsAssigned/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<int>(tickets);
            }

            return 0;
        }

        public int GetCantTicketsOpen()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = _httpClient.GetAsync("api/Report/CantTicketsOpen").Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<int>(tickets);
            }

            return 0;
        }
    }
}
