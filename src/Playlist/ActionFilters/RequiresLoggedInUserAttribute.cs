using System.Web.Mvc;
using Playlist.Data.Dtos;

namespace Playlist.ActionFilters
{
    /// <summary>
    /// Attribute used to decorate action methods that require a logged in user.
    /// </summary>
    public class RequiresLoggedInUserAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// A key in TempData that's set when this attribute redirects to the login page.
        /// </summary>
        public const string TempDataKey = "UserNotLoggedIn";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check for a logged in user and if one isn't found, redirect to login page
            var user = (UserDto) filterContext.RequestContext.HttpContext.Session["user"];
            if (user == null)
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                filterContext.Controller.TempData[TempDataKey] = true;
                filterContext.Result = new RedirectResult(urlHelper.Action("Index", "Login"));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}