using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class SupervisorModel : ISupervisorModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SupervisorModel(IConfiguration config, IHttpContextAccessor httpContextAccessor) {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool DeleteSupervisorById(int id)
        {
            
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.DeleteAsync("api/User/"+ id).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public User GetSupervisorById(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/User/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var supervisor = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<User>(supervisor);
            }

            return null;
        }
    
        public List<User> GetSupervisors()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/User/Supervisor").Result;

            if (response.IsSuccessStatusCode)
            {
                var supervisors = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<User>>(supervisors);
            }

            return null;
        }

        public async Task<int> GetSupervisorSucursal(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/User/GetSupervisorSucursal/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var supervisor = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<int>(supervisor);
                 
            }

            return 0;
        }

        public bool PostSupervisor(User user)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");


            var response = _httpClient.PostAsync("api/User/", content).Result;

            if (response.IsSuccessStatusCode)
            {  
                return true;
            }

            return false;
        }

        public bool PutSupervisorById(User user)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");


            var response = _httpClient.PutAsync("api/User/" + user.id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
