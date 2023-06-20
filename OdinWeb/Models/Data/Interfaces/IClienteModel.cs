using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IClienteModel
    {
        public List<User> GetClients();

        public bool PostClient(User user);

        public User GetClientById(int id);

        public bool PutClientById(User user);

        public bool DeleteClientById(int id);

    }
}
