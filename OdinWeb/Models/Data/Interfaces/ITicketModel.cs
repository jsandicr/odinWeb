using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface ITicketModel
    {
        public List<Ticket> GetTickets();
        public List<Ticket> GetAssignedTickets(string status);
        public List<Ticket> GetOpenTickets();
        public Ticket PostTicket(Ticket ticket);
        public Ticket GetTicketById(int id);
        public bool PutTicketById(Ticket ticket);
        public bool DeleteTicketById(int id);
        public List<Ticket> GetTicketsClientsStatus(string status);
    }
}
