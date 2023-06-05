using Microsoft.AspNetCore.Mvc;

namespace OdinWeb.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult UsuariosConsulta()
        //{
        //    var resultado = modelUsuario.Consultar_Usuarios_Estado(-1);

        //    if (resultado != null && resultado.Codigo == 1)
        //        return View(resultado);
        //    else
        //        return View("Error");
        //}

    }
}
