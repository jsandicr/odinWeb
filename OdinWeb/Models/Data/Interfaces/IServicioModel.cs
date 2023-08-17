using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IServicioModel
    {
        public List<Service> GetServicios();
        public List<Service> GetServiciosStatus(bool status);
        public bool PostServicos(Service service);
        public Service GetServicioById(int? id);
        public bool PutServicioById(Service service);
        public bool DeleteServicioById(int id);
        public List<Service> GetListSubServicioById(long id);
        public List<Service> GetFinalServices();
    }
}