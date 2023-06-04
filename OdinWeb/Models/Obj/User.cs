namespace OdinWeb.Models.Obj
{
    public class User
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string mail { get; set; }
        public string phone { get; set; }
        public string photo { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public int idRol { get; set; }
        public int idBranch { get; set; }
        public string? token { get; set; }
        public Rol? rol { get; set; }
        public Branch? branch { get; set; }
        public List<Ticket>? ticketsS { get; set; }
        public List<Ticket>? ticketsC { get; set; }
        public List<Comment>? comments { get; set; }
        public List<ErrorLog>? errorsLog { get; set; }
        public List<TransactionalLog>? transactionsLog { get; set; }
    }
}
