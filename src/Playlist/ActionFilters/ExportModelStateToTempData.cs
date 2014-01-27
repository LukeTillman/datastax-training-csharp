using System.Web.Mvc;

namespace Playlist.ActionFilters
{
    /// <summary>
    /// Exports the current ModelState to TempData.  Useful for transerring model validation errors between pages
    /// when using the Post-Redirect-Get pattern.  The ModelState will only be transferred if the state is invalid, and
    /// a RedirectResult or RedirectToRouteResult is being returned by the action.
    /// </summary>
    public class ExportModelStateToTempData : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Only export when ModelState is not valid
            if (filterContext.Controller.ViewData.ModelState.IsValid == false)
            {
                //Export if we are redirecting
                if (filterContext.Result is RedirectResult || filterContext.Result is RedirectToRouteResult || filterContext.Result is JsonResult)
                {
                    filterContext.Controller.TempData[Key] = filterContext.Controller.ViewData.ModelState;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}