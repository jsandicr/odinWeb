using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IStatusModel
    {
        public List<Status> GetStatus();

        public bool PostStatus(Status status);

        public Status GetStatusById(int id);

        public bool PutStatusById(Status status);

        public bool DeleteStatusById(int id);

    }
}
