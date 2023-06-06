namespace OdinWeb.Models.Obj
{
    public class Service
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
        public List<Ticket>? tickets { get; set; }
    }
}
