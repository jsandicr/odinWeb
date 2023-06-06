namespace OdinWeb.Models.Obj
{
    public class Branch
    {
        public int id { get; set; }
        public string name { get; set; }
        public string direction { get; set; }
        public bool active { get; set; }
        public List<User>? users { get; set; }
    }
}
