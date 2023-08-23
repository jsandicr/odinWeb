using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;

namespace OdinWeb.Models.Data.Classes
{
    public class TransLogModel : ITransLogModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransLogModel(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> DeleteAsync(int days)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync("api/TransactionalLog/"+days);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<List<TransactionalLog>> GetAsync()
        {

            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/TransactionalLog");

            if (response.IsSuccessStatusCode)
            {
                var logs = await response.Content.ReadAsStringAsync();
                var logsR = JsonConvert.DeserializeObject<List<TransactionalLog>>(logs);
                return logsR;
            }
            return null;
        }

        public async Task<List<ErrorLog>> GetAsyncE()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("api/ErrorLog");

            if (response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                var errorR = JsonConvert.DeserializeObject<List<ErrorLog>>(error);
                return errorR;
            }
            return null;
            throw new NotImplementedException();
        }
    }
}
