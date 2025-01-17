﻿using Newtonsoft.Json;
using OdinApi.Models.Obj;
using OdinWeb.Models.Data.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class DocumentModel : IDocumentModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DocumentModel(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Documento> DeleteDocuemnt(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync("api/Document/" + id);

            if (response.IsSuccessStatusCode)
            {
                var documentoJson = await response.Content.ReadAsStringAsync();
                var documento = JsonConvert.DeserializeObject<Documento>(documentoJson);
                return documento;
            }

            return null;
        }

        public bool PostDocument(Documento Document)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(Document), Encoding.UTF8, "application/json");


            var response = _httpClient.PostAsync("api/Document", content).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
