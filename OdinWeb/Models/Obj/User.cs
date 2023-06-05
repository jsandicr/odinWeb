using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string photo { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public int idRol { get; set; }
        public int idBranch { get; set; }
        public string? token { get; set; }
        public Rol? rol { get; set; }
        public Branch? branch { get; set; }
        public List<Ticket>? ticketsS { get; set; }
        public List<Ticket>? ticketsC { get; set; }
        public List<Comment>? comments { get; set; }
        public List<ErrorLog>? errorsLog { get; set; }
        public List<TransactionalLog>? transactionsLog { get; set; }
    }
    public class RestorePassword
    {
        [Required(ErrorMessage = "El campo 'mail' es requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo 'mail' debe ser una dirección de correo electrónico válida")]
        public string mail { get; set; }

        [Required(ErrorMessage = "El campo 'phone' es requerido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo 'phone' debe ser un número de teléfono válido")]
        public string phone { get; set; }
    }

    public class LoginViewModel
    {
        public UserDTO User { get; set; }
        public RestorePassword RestorePassword { get; set; }
    }
}
