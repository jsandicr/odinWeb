namespace OdinWeb.Models.Obj
{
    public class TransactionalLog
    {
        public int id { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string module { get; set; }
        public DateTime date { get; set; }
        public int idUser { get; set; }
        public User? user { get; set; }
    }
}
