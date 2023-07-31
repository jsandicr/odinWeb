using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IReportModel
    {
        public List<Ticket> GetTicketsXTime();
        public List<Ticket> GetTicketsXSupervisor();
        public int GetCantTicketsAssigned();
        public int GetCantTicketsOpen();
    }
}