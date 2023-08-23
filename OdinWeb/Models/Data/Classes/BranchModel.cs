using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class BranchModel : IBranchModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BranchModel(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public List<Branch> GetBranch()
        {  

            var response = _httpClient.GetAsync("api/Branch").Result;

            if (response.IsSuccessStatusCode)
            {
                var branch = response.Content.ReadAsStringAsync().Result;
                var branchR = JsonConvert.DeserializeObject<List<Branch>>(branch);
                return branchR;
            }

            return null;
        }

        public bool DeleteBranchById(int id)
        {

            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.DeleteAsync("api/Branch/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public Branch GetBranchById(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Branch/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var branch = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Branch>(branch);
            }

            return null;
        }

        public bool PostBranch(Branch branch)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(branch), Encoding.UTF8, "application/json");


            var response = _httpClient.PostAsync("api/Branch", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public bool PutBranchById(Branch branch)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(branch), Encoding.UTF8, "application/json");

            var response = _httpClient.PutAsync("api/Branch/" + branch.id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public List<Branch> GetBranchesAll()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = _httpClient.GetAsync("api/Branch/All").Result;

            if (response.IsSuccessStatusCode)
            {
                var branch = response.Content.ReadAsStringAsync().Result;
                var branchR = JsonConvert.DeserializeObject<List<Branch>>(branch);
                return branchR;
            }

            return null;
        }
    }
}
