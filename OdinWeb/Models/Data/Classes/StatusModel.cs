using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class StatusModel : IStatusModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StatusModel(IConfiguration config, IHttpContextAccessor httpContextAccessor) {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool DeleteStatusById(int id)
        {
            
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.DeleteAsync("api/Status/"+ id).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    

        public Status GetStatusById(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Status/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var services = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Status>(services);
            }

            return null;
        }
    

        public List<Status> GetStatus()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Status").Result;

            if (response.IsSuccessStatusCode)
            {
                var status = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Status>>(status);
            }

            return null;
        }


        public bool PostStatus(Status status)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(status), Encoding.UTF8, "application/json");


            var response = _httpClient.PostAsync("api/Status", content).Result;

            if (response.IsSuccessStatusCode)
            {
                
                return true;
            }

            return false;
        }

        public bool PutStatusById(Status status)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(status), Encoding.UTF8, "application/json");


            var response = _httpClient.PutAsync("api/Status/" + status.id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                
                return true;
            }

            return false;

        }
    }
}
