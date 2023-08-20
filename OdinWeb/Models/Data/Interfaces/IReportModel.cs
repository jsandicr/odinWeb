using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IReportModel
    {
        public List<Ticket> GetTicketsXTime(DateTime date1, DateTime date2);
        public List<Ticket> GetTicketsXSupervisor(DateTime date1, DateTime date2);
        public List<Ticket> GetTicketsXSupervisorM();
        public int GetCantTicketsAssigned();
        public int GetCantTicketsOpen();
    }
}