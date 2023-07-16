using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;

namespace OdinWeb.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentModel _documentModel;
        public DocumentController(IDocumentModel documentModel)
        {
            _documentModel = documentModel;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(int id, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                var respuesta = await _documentModel.DeleteDocuemnt(id);
                if (respuesta !=null)
                {
                    var rutaDoc = Path.Combine(hostingEnvironment.WebRootPath, "Document", respuesta.name);
                    if (System.IO.File.Exists(rutaDoc))
                    {
                        System.IO.File.Delete(rutaDoc);
                    }
                    return Ok(true);
                }
                return Ok(false);
            }
            catch
            {
                return Ok(false);
            }
        }
    }
}
