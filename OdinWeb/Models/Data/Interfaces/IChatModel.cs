using OdinWeb.Models.Obj;

namespace OdinWeb.Models.Data.Interfaces
{
    public interface IChatModel
    {
        public List<Chat> GetChat();
        public Chat GetChatById(int id);
        public bool DeleteChatById(int id);
        public bool PutChatById(Chat chat);
        public bool PostChat(Chat chat);
    }
}
