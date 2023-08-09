using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface ISupervisorModel
    {
        public List<User> GetSupervisors();

        public bool PostSupervisor(User user);

        public User GetSupervisorById(int id);

        public bool PutSupervisorById(User user);

        public bool DeleteSupervisorById(int id);

        Task<User> GetSupervisorSucursal(int id);

    }
}
