using System.ComponentModel.DataAnnotations;
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
}
