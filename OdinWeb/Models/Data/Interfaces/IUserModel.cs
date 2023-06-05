using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IUserModel
    {
        Task<bool> RestorePassword(RestorePassword user);
    }
}

