using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DayRun.Web.Filters
{
    public class RoleAuthorize : ActionFilterAttribute
    {
        private readonly string _role;
        public RoleAuthorize(string role = "") { _role = role; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var sessionRole = context.HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(sessionRole))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else if (!string.IsNullOrEmpty(_role) && sessionRole != _role)
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}