using OdinWeb.Models.Obj;

namespace OdinApi.Models.Obj
{
    public class Documento
    {
        public long id { get; set; }
        public string name { get; set; }
        public int idUser { get; set; }
        public int idTicket { get; set; }
        public string nameDocument { get; set; }
        public User? user { get; set; }
        public Ticket? ticket { get; set; }
    }
}
