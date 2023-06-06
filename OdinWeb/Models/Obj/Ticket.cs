namespace OdinWeb.Models.Obj
{
    public class Ticket
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime updateDate { get; set; }
        public DateTime closeDate { get; set; }
        public DateTime estimatedDate { get; set; }
        public bool active { get; set; }
        public int idClient { get; set; }
        public int idSupervisor { get; set; }
        public int idService { get; set; }
        public int idStatus { get; set; }
        public User? client { get; set; }
        public User? supervisor { get; set; }
        public Service? service { get; set; }
        public Status? status { get; set; }
        public List<Comment>? comments { get; set; }
    }
}
