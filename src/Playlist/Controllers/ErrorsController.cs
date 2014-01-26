using System.Net;
using System.Web.Mvc;

namespace Playlist.Controllers
{
    public class ErrorsController : Controller
    {
        /// <summary>
        /// Displays the View for 404 not found errors.
        /// </summary>
        public ActionResult NotFound()
        {
            Response.StatusCode = (int) HttpStatusCode.NotFound;
            return View();
        }
    }
}