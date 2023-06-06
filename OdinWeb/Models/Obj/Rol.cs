namespace OdinWeb.Models.Obj
{
    public class Rol
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
        public List<User>? users { get; set; }
    }
}
