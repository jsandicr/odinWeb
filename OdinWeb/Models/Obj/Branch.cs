using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace OdinWeb.Models.Obj
{
    public class Branch
    {
        public int id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(100, ErrorMessage = "El campo no puede contener más de 500 caracteres")]
        public string name { get; set; }
        [Display(Name = "Dirección")]

        [Required(ErrorMessage = "El campo es requerido")]
        [StringLength(1000, ErrorMessage = "El campo no puede contener más de 500 caracteres")]
        public string direction { get; set; }
        [Display(Name = "Estado")]
        [Required(ErrorMessage = "El campo es requerido")]
        public bool active { get; set; }
        public List<User>? users { get; set; }
    }
}
