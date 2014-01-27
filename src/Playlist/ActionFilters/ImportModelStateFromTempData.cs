using System.Web.Mvc;

namespace Playlist.ActionFilters
{
    /// <summary>
    /// Imports ModelState from TempData.  Useful when using the Post-Redirect-Get pattern to transfer ModelState errors from
    /// a failed post to the resulting page being requested by the Get.  Will only import ModelState from TempData if it was
    /// previously exported and if the Action is returning a ViewResult.
    /// </summary>
    public class ImportModelStateFromTempData : ModelStateTempDataTransfer
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var modelState = filterContext.Controller.TempData[Key] as ModelStateDictionary;
            if (modelState != null)
            {
                // Only Import if we are viewing
                if (filterContext.Result is ViewResult)
                {
                    filterContext.Controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    // Otherwise remove it
                    filterContext.Controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}