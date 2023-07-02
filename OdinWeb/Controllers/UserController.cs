using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net;
using System.Net.Http.Headers;

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
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            
            var idU = _httpContextAccessor.HttpContext.Request.Cookies["Id"];
            int id = int.Parse(idU);
            var u = _userModel.GetUserById(id);
            List<Branch> branches = _branchModel.GetBranch();

            List<SelectListItem> branchesOps = new List<SelectListItem>();
            foreach (Branch branch in branches)
            {
                branchesOps.Add(new SelectListItem
                {
                    Text = branch.name,
                    Value = branch.id.ToString()
                });
            };
            ViewData["Branches"] = branchesOps;
            UpdateUser user = new UpdateUser();
            user.id = u.id;
            user.Nombre = u.name;
            user.Apellidos = u.lastName;
            user.CorreoElectronico = u.mail;
            user.Telefono = u.phone;
            user.rutaImagen = u.photo;
            user.IdBranch = (int)u.idBranch;

            return View(user);
        }

        public IActionResult GuardarDatos(UpdateUser u, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            var user = _userModel.GetUserById(u.id);

            user.idBranch = u.IdBranch;
            user.phone = u.Telefono;
            user.mail = u.CorreoElectronico;

            var archivoImagen = u.Imagen;

            if (archivoImagen != null && archivoImagen.Length > 0)
            {
                var nombreArchivo = Path.GetFileName(archivoImagen.FileName);
                var extension = Path.GetExtension(nombreArchivo);
                if (extension == ".png" || extension == ".jpg")
                {
                    if (!string.IsNullOrEmpty(user.photo))
                    {
                        var rutaImagenAnterior = Path.Combine(hostingEnvironment.WebRootPath, "images", "users", user.photo);
                        if (System.IO.File.Exists(rutaImagenAnterior))
                        {
                            System.IO.File.Delete(rutaImagenAnterior);
                        }
                    }
                    var nombreUnico = Guid.NewGuid().ToString() + extension;

                    var rutaGuardar = Path.Combine(hostingEnvironment.WebRootPath, "images", "users", nombreUnico);
                    using (var stream = new FileStream(rutaGuardar, FileMode.Create))
                    {
                        archivoImagen.CopyTo(stream);
                    }

                    user.photo = nombreUnico;

                }
                else {
                    TempData["AlertMessage"] = "Error, solo se permiten archivos .png o .jpg";
                    TempData["AlertType"] = "error";
                    return RedirectToAction("Profile");

                }
           
            }
            var respuesta = _userModel.PutUserById(user);
            if (respuesta)
            {
                TempData["AlertMessage"] = "Datos actulizados correctamente";
                TempData["AlertType"] = "success";
                return RedirectToAction("Profile");
            }
            TempData["AlertMessage"] = "Error verfique los datos";
            TempData["AlertType"] = "error";
            return RedirectToAction("Profile");

        }

    }
}
