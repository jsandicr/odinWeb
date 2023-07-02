using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OdinWeb.Models.Obj
{
    public class Service
    {
        public int id { get; set; }
        [Display(Name = "Nombre")]
        public string name { get; set; }
        [Display(Name = "Descripcion")]
        public string description { get; set; }
        [Display(Name = "Activo")]
        public bool active { get; set; }
        public string photo { get; set; }
        public List<Ticket>? tickets { get; set; }
    }

    public class ServiceUDP
    {
        public int id { get; set; }
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo es requerido")]

        public string name { get; set; }
        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "El campo es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre de usuario no puede tener más de 200 caracteres.")]
        public string description { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]

        [Display(Name = "Activo")]

        public bool active { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public string photo { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public IFormFile image { get; set; }
        public List<Ticket>? tickets { get; set; }
    }
}
