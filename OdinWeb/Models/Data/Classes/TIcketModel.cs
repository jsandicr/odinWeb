﻿using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Models.Data.Classes
{
    public class TicketModel : ITicketModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TicketModel(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(_config["ApiSettings:BaseUrl"]); // URL base del API
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public bool DeleteTicketById(int id)
        {
            
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.DeleteAsync("api/Ticket/"+ id).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public Ticket GetTicketById(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Ticket/" + id).Result;

            if (response.IsSuccessStatusCode)
            {
                var client = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Ticket>(client);
            }

            return null;
        }
    
        public List<Ticket> GetTickets()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Ticket").Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }

        public List<Ticket> GetAssignedTickets(string status)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            var id = _httpContextAccessor.HttpContext.Request.Cookies["Id"];

            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Ticket/Assigned/"+ id + "," + status).Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }

        public List<Ticket> GetOpenTickets()
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Ticket/Open").Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }


        public Ticket PostTicket(Ticket ticket)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(ticket), Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync("api/Ticket", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Ticket>(tickets);
            }

            return null;
        }

        public bool PutTicketById(Ticket ticket)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(ticket), Encoding.UTF8, "application/json");


            var response = _httpClient.PutAsync("api/Ticket/" + ticket.id, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public List<Ticket> GetTicketsClientsStatus(string status)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            var id = _httpContextAccessor.HttpContext.Request.Cookies["Id"];

            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Ticket/GetTicketsClients/" + id + "," + status).Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }

        public async Task<List<Ticket>> GetTicketsByBranch(int branchId, string status)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];

            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Ticket/Sucursal/" + branchId + "," + status).Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }

        public async Task<List<Ticket>> GetTicketsByStatus(string status)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];

            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Ticket/Status/" + status).Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }

        public async Task<List<Ticket>> GetTiketViewBySelect(int select, int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];

            // Agrega el encabezado de autorización con el token
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = _httpClient.GetAsync("api/Ticket/GetTiketViewBySelect/" + select+","+id).Result;

            if (response.IsSuccessStatusCode)
            {
                var tickets = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<List<Ticket>>(tickets);
            }

            return null;
        }
    }
}
