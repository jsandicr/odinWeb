using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class TransactionalLog
    {
        public int id { get; set; }
        [Display(Name = "Tipo")]
        public string type { get; set; }
        [Display(Name = "Descripción")]
        public string description { get; set; }
        [Display(Name = "Módulo")]
        public string module { get; set; }
        [Display(Name = "Fecha")]
        public DateTime date { get; set; }
        [Display(Name = "Usuario")]
        public int idUser { get; set; }
        [Display(Name = "Usuario")]
        public User? user { get; set; }
    }
}