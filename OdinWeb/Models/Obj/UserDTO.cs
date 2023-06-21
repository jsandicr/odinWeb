using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class UserDTO
    {
        [Required(ErrorMessage = "El campo es requerido")]
        [EmailAddress(ErrorMessage = "El campo Correo Electronico debe ser una dirección de correo electrónico válida")]
        [Display(Name = "Email")]
        public string mail { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
    }
}
