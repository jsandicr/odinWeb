using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net;

namespace OdinWeb.Controllers
{
    public class UserController : Controller
    {
        readonly IBranchModel _branchModel;
        readonly IUserModel _userModel;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IBranchModel branchModel, IUserModel userModel, IHttpContextAccessor httpContextAccessor) { 
            _branchModel = branchModel;
            _userModel = userModel;
            _httpContextAccessor = httpContextAccessor;
        }


        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Settings()
        {
            List<Branch> comboBranch = _branchModel.GetBranch();
            ViewBag.ComboBranch = comboBranch.Select(t => new SelectListItem
            {
                Value = t.id.ToString(),
                Text = t.name
            }).ToList();
            var idU = _httpContextAccessor.HttpContext.Request.Cookies["Id"];
            int id = int.Parse(idU);
            var u = _userModel.GetUserById(id);
            UpdateUser user = new UpdateUser();
            user.id = u.id;
            user.Nombre = u.name;
            user.Apellidos = u.lastName;
            user.CorreoElectronico = u.mail;
            user.Telefono = u.phone;
            user.rutaImagen = u.photo;

            return View(user);
        }

        public IActionResult GuardarDatos(UpdateUser u, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            var user = _userModel.GetUserById(u.id);

            user.idBranch = u.IdBranch;
            user.phone = u.Telefono;

            var archivoImagen = u.ArchivoImagen;

            if (archivoImagen != null && archivoImagen.Length > 0)
            {
                var nombreArchivo = Path.GetFileName(archivoImagen.FileName);
                var extension = Path.GetExtension(nombreArchivo);
                var nombreUnico = Guid.NewGuid().ToString() + extension;

                var rutaGuardar = Path.Combine(hostingEnvironment.WebRootPath, "images", "users", nombreUnico);
                using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                {
                    archivoImagen.CopyTo(stream);
                }

                user.photo = nombreUnico;
                
            }
            _userModel.PutUserById(user);
            return RedirectToAction("Settings");

            // Si no se subió ningún archivo, puedes mostrar un mensaje de error o realizar alguna otra acción


            // Si el modelo no es válido, puedes realizar alguna acción, como volver a mostrar el formulario con los mensajes de error
            
        }

    }
}
