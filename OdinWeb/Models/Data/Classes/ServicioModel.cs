using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class ServicioModel : IServicioModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ServicioModel(IConfiguration config, IHttpContextAccessor httpContextAccessor) {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool DeleteServicioById(int id)
        {
            
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.DeleteAsync("api/Service/"+ id).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    

        public Service GetServicioById(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
             _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Service/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var services = response.Content.ReadAsStringAsync().Result;
                var servicesR = JsonConvert.DeserializeObject<Service>(services);
                return servicesR;
            }

            return null;
        }
    

        public List<Service> GetServicios()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Service").Result;

            if (response.IsSuccessStatusCode)
            {
                var services = response.Content.ReadAsStringAsync().Result;
                var servicesR = JsonConvert.DeserializeObject<List<Service>>(services);
                return servicesR;
            }

            return null;
        }

        public List<Service> GetServiciosStatus(bool status)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Service/status/" + status).Result;

            if (response.IsSuccessStatusCode)
            {
                var services = response.Content.ReadAsStringAsync().Result;
                var servicesR = JsonConvert.DeserializeObject<List<Service>>(services);
                return servicesR;
            }

            return null;
        }

        public List<Service> GetListSubServicioById(long id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Service/ListSubSerice/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var services = response.Content.ReadAsStringAsync().Result;
                var servicesR = JsonConvert.DeserializeObject<List<Service>>(services);
                return servicesR;
            }

            return null;
        }

        public bool PostServicos(Service service)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(service), Encoding.UTF8, "application/json");


            var response = _httpClient.PostAsync("api/Service", content).Result;

            if (response.IsSuccessStatusCode)
            {
                
                return true;
            }

            return false;
        }

        public bool PutServicioById(Service service)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(service), Encoding.UTF8, "application/json");


            var response = _httpClient.PutAsync("api/Service/" + service.id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                
                return true;
            }

            return false;

        }
    }
}
