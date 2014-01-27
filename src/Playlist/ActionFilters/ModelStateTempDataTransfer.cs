using System.Web.Mvc;

namespace Playlist.ActionFilters
{
    /// <summary>
    /// Base class for the attributes for importing/exporting ModelState to TempData.
    /// </summary>
    public abstract class ModelStateTempDataTransfer : ActionFilterAttribute
    {
        protected static readonly string Key = typeof (ModelStateTempDataTransfer).FullName;
    }
}