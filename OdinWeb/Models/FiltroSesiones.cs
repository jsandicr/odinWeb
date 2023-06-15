using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace OdinWeb.Models
{
    public class FiltroSesiones : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("Token") == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
            {
                { "controller", "Auth" },
                { "action", "Login" }
            });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}