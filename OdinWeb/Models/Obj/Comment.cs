namespace OdinWeb.Models.Obj
{
    public class Comment
    {
        public int id { get; set; }
        public string description { get; set; }
        public DateTime date { get; set; }
        public bool active { get; set; }
        public int idUser { get; set; }
        public int idTicket { get; set; }
        public User? user { get; set; }
        public Ticket? ticket { get; set; }
    }
}
