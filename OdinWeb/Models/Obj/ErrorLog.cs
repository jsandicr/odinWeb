using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class ErrorLog
    {
        public int id { get; set; }
        [Display(Name = "Código")]
        public int code { get; set; }

        [Display(Name = "Descripción")]
        public string description { get; set; }

        [Display(Name = "Fecha")]
        public DateTime date { get; set; }

        [Display(Name = "ID de Usuario")]
        public int? idUser { get; set; }

        [Display(Name = "Usuario")]
        public User? user { get; set; }
    }
}
