using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IUserModel
    {
        Task<bool> RestorePassword(RestorePassword user);

        public User ChangePassword(ChangePassword user);
        string HashPassword(string password);

        public User Login(UserDTO userDTO);

        public User GetUserById(int id);

        public bool PutUserById(User user);

        public bool PostUser(User user);

    }
}

