using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class UserModel : IUserModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserModel(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            

        }
        public async Task<bool> RestorePassword(RestorePassword user)
        {
            var respuesta = false;
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/User/restorePassword", content);
            

            if (response.IsSuccessStatusCode)
            {
                respuesta = true;
            }

            return respuesta;
        }

        public string HashPassword(string password)
        {

            byte[] fixedSalt = new byte[128 / 8];
            //Se utiliza un valor fijo temporalmente
            var salt = Encoding.UTF8.GetBytes("1234567890abcdef");

            // Generar el hash de la contraseña utilizando PBKDF2
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return hashed;
        }

        public User Login(UserDTO userDTO)
        {

            var content = new StringContent(JsonConvert.SerializeObject(userDTO), Encoding.UTF8, "application/json");
            var response = _httpClient.PostAsync("api/User/Login", content).Result;


            if (response.IsSuccessStatusCode)
            {
                var userJson = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<User>(userJson);
                return user;
            }

            return null;

        }

        public User ChangePassword(ChangePassword user)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respuesta = false;
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var response =  _httpClient.PutAsync("api/User/changePasswordd", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var userR = response.Content.ReadAsStringAsync().Result;
                var userD = JsonConvert.DeserializeObject<User>(userR);
                return userD;
            }

            return null;

        }
    }
}
