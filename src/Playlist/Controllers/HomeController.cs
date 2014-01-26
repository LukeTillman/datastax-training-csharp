using System;
using System.Web.Mvc;
using Playlist.Data;
using Playlist.Models.Home;

namespace Playlist.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICassandraInfo _cassandraInfo;

        public HomeController(ICassandraInfo cassandraInfo)
        {
            if (cassandraInfo == null) throw new ArgumentNullException("cassandraInfo");
            _cassandraInfo = cassandraInfo;
        }

        /// <summary>
        /// Displays the Playlist home page.
        /// </summary>
        public ActionResult Index()
        {
            return View(new IndexModel
            {
                CassandraClusterName = _cassandraInfo.GetClusterName(),
                CassandraVersion = _cassandraInfo.GetCassandraVersion(),
                ClrVersion = Environment.Version.ToString()
            });
        }
    }
}
