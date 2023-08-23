using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OdinWeb.Models.Data.Interfaces;

namespace OdinWeb.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentModel _documentModel;
        private IWebHostEnvironment _env;

        public DocumentController(IDocumentModel documentModel, IWebHostEnvironment env)
        {
            _documentModel = documentModel;
            _env = env;
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
        [HttpGet]
        [Authorize]
        public IActionResult ViewDocumentManual()
        {
            var webRootPath = _env.WebRootPath; // Obtener la ruta raíz del servidor
            var documentPath = Path.Combine(webRootPath, "Manual", "GuiaUsuarioODIN.pdf"); // Combinar la ruta raíz con la ruta relativa del documento

            if (!System.IO.File.Exists(documentPath))
            {
                return NotFound();
            }

            string fileName = Path.GetFileName(documentPath);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out string contentType))
            {
                contentType = "application/octet-stream";
            }

            FileStream fileStream = new FileStream(documentPath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, contentType);
        }
    }
}
