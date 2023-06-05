using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class UserDTO
    {
        [Required(ErrorMessage = "El campo de Correo Electronico es requerido")]
        [EmailAddress(ErrorMessage = "El campo Correo Electronico debe ser una dirección de correo electrónico válida")]
        public string mail { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo Contraseña debe tener entre 6 y 20 caracteres")]
        public string password { get; set; }
    }
}
