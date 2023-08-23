using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class CommentModel : ICommentModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommentModel(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<bool> PostComment(string mensaje, int id)
        {
            Comment comment = new Comment();
            comment.description = mensaje;
            comment.idTicket = id;
            comment.idUser = int.Parse(_httpContextAccessor.HttpContext.Request.Cookies["Id"]);
            comment.active = true;
            comment.date= DateTime.Now;
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");


            var response = _httpClient.PostAsync("api/Comment", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }

}
