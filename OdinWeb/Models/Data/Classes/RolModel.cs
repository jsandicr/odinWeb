using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;

namespace OdinWeb.Models.Data.Classes
{
    public class RolModel : IRolModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RolModel(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Rol GetRolFirst()
        {
            var response = _httpClient.GetAsync("api/Rol/Firs").Result;

            if (response.IsSuccessStatusCode)
            {
                var rol = response.Content.ReadAsStringAsync().Result;
                var rolR = JsonConvert.DeserializeObject<Rol>(rol);
                return rolR;
            }

            return null;
        }
    }
}
