namespace OdinWeb.Models.Data.Interfaces
{
    public interface ICommentModel
    {

        Task<bool> PostComment(string mensaje,int id);

    }
}
