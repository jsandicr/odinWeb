using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class User
    {
        public int id { get; set; }
        [Display(Name = "Nombre")]
        public string name { get; set; }
        [Display(Name = "Apellido")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "El campo 'Email' es requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo 'Email' debe ser una dirección de correo electrónico válida")]
        [Display(Name = "Email")]
        public string mail { get; set; }
        [Required(ErrorMessage = "El campo 'phone' es requerido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo 'phone' debe ser un número de teléfono válido")]
        [Display(Name = "Telefono")]
        public string phone { get; set; }
        public string photo { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es requerido")]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "El campo Contraseña debe tener entre 6 y 20 caracteres")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
        [Display(Name = "Activo")]
        public bool active { get; set; } = true;
        public bool restorePass { get; set; } = false;
        [Display(Name = "Id Rol")]
        public int idRol { get; set; }
        [Display(Name = "Id Sucursal")]
        public int? idBranch { get; set; }
        public string? token { get; set; }
        [Display(Name = "Rol")]
        public Rol? rol { get; set; }
        [Display(Name = "Branch")]
        public Branch? branch { get; set; }
        public List<Ticket>? ticketsS { get; set; }
        public List<Ticket>? ticketsC { get; set; }
        public List<Comment>? comments { get; set; }
        public List<ErrorLog>? errorsLog { get; set; }
        public List<TransactionalLog>? transactionsLog { get; set; }
    }

    public class UserRegister
    {
        [Display(Name = "Nombre")]
        public string name { get; set; }
        [Display(Name = "Apellido")]
        public string lastName { get; set; }
        [Required(ErrorMessage = "El campo 'Email' es requerido")]
        [DataType(DataType.EmailAddress, ErrorMessage = "El campo 'Email' debe ser una dirección de correo electrónico válida")]
        [Display(Name = "Correo")]
        public string mail { get; set; }
        [Required(ErrorMessage = "El campo 'phone' es requerido")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "El campo 'phone' debe ser un número de teléfono válido")]
        [Display(Name = "Telefono")]
        public string phone { get; set; }
        [Required(ErrorMessage = "El campo Contraseña es requerido")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo Contraseña debe tener entre 6 y 20 caracteres")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
        [Display(Name = "Activo")]
        public bool active { get; set; }
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
    public class ChangePassword
    {
        [HiddenInput(DisplayValue = false)]
        public int id { get; set; }

        [Required(ErrorMessage = "El campo es requerido.")]
        [Display(Name = "Contraseña Actual")]
        public string oldPassword { get; set; }


        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo Contraseña debe tener entre 6 y 20 caracteres")]
        [Display(Name = "Nueva Contraseña")]
        [Required(ErrorMessage = "El campo es requerido.")]
        public string password { get; set; }

        [Display(Name = "Confirnar Contraseña")]
        [Compare("password", ErrorMessage = "Las contraseñas no coinciden.")]
        [Required(ErrorMessage = "El campo es requerido.")]
        public string confirmpassword { get; set; }
    }
}
