using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class Chat
    {
        public int Id { get; set; }

        [Display(Name = "Pregunta")]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(300, ErrorMessage = "El campo no puede contener más de 300 caracteres")]
        public string Text { get; set; }

        [Display(Name = "Respuesta")]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(1000, ErrorMessage = "El campo no puede contener más de 1000 caracteres")]
        public string Answer { get; set; }
    }
}
