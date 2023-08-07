using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OdinWeb.Models;
using OdinWeb.Models.Data.Classes;
using OdinWeb.Models.Data.Interfaces;
using OdinWeb.Models.Obj;
using System.Net.Http.Headers;
using System.Text;

namespace OdinWeb.Controllers
{
    public class ChatController : Controller
    {

        private readonly IChatModel _chatModel;

        public ChatController(IChatModel chatModel)
        {
            _chatModel = chatModel;
        }



        [Authorize]
        public IActionResult Home()
        {
            try
            {
                var lista = _chatModel.GetChat();
                if (lista != null)
                {
                    return View(lista);

                }
                return View();
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize]
        public IActionResult Ver(int id)
        {
            try
            {
                var chat = _chatModel.GetChatById(id);
                if (chat != null)
                {
                    return View(chat);

                }
                return RedirectToAction(nameof(Home));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Home));
            }
        }

        [Authorize]
        public async Task<IActionResult> Crear()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Guardar(Chat chat)
        {
            try
            {

                var servicio = _chatModel.PostChat(chat);

                if (servicio)
                {
                    TempData["AlertMessage"] = "¡Se creó el chat!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));

                }
                
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el chat!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al crear el chat!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Crear));
            }
        }

        [Authorize]
        public IActionResult Editar(int id)
        {
            try
            {
                var chat = _chatModel.GetChatById(id);
                if (chat != null)
                {
                    return View(chat);

                }
                return RedirectToAction(nameof(Home));
            }
            catch (Exception e)
            {
                return RedirectToAction(nameof(Home));
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Actualizar(Chat chat)
        {
            try
            {

                var servicio = _chatModel.PutChatById(chat);

                if (servicio)
                {
                    TempData["AlertMessage"] = "¡Se actualizó el chat!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));
                }

                
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el chat!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al actualizar el chat!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Editar));
            }
        }

        [Authorize]
        public IActionResult Eliminar(int id)
        {
            try
            {
                var respuesta = _chatModel.DeleteChatById(id);

                if (respuesta)
                {
                    TempData["AlertMessage"] = "¡Se eliminó el chat!";
                    TempData["AlertType"] = "success";
                    return RedirectToAction(nameof(Home));
                }
                TempData["AlertMessage"] = "¡Ocurrio un error al eliminar el chat!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
            catch
            {
                TempData["AlertMessage"] = "¡Ocurrio un error al eliminar el chat!";
                TempData["AlertType"] = "error";
                return RedirectToAction(nameof(Home));
            }
        }


    }
}
