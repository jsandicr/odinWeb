using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface ITransLogModel
    {
        Task<List<TransactionalLog>> GetAsync();
    }
}
