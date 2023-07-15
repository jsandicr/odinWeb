using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IDocumentModel
    {
        public bool PostDocument(Document Document);

        Task<Documento> DeleteDocuemnt(int id);

    }
}
