using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Data.Interfaces;

namespace OdinWeb.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentModel _comment;

        public CommentController(ICommentModel comment) { 
         _comment = comment;
        }

        [HttpPost]
        [Authorize]
        public async Task<bool> AddComment(string mjs, int id)
        {
            try { 
               var respuesta = await _comment.PostComment(mjs, id);
                if (respuesta) {
                    return true;
                }
                return false;
            }
            catch {
                return false;

            }
        }
    }
}
