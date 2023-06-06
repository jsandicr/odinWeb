namespace OdinWeb.Models.Obj
{
    public class ErrorLog
    {
        public int id { get; set; }
        public int code { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public int idUser { get; set; }
        public User? user { get; set; }
    }
}
