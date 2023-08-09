using System.ComponentModel.DataAnnotations;

namespace OdinWeb.Models.Obj
{
    public class Chat
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
    }
}
