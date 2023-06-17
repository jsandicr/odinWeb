using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Net.Http;

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
    }
}
