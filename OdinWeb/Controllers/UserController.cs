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
            return View(_userModel.GetUserById(id));
        }

    }
}
