using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class UserDTO
    {
        [Required(ErrorMessage = "El es requerido")]
        [EmailAddress(ErrorMessage = "El campo Correo Electronico debe ser una dirección de correo electrónico válida")]
        [Display(Name = "Email")]
        public string mail { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(int.MaxValue, MinimumLength = 6, ErrorMessage = "El campo Contraseña debe tener entre 6 y 20 caracteres")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
    }
}
