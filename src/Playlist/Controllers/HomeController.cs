using System;
using System.Web.Mvc;
using Playlist.Data;
using Playlist.Models.Home;

namespace Playlist.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICassandraInfo _cassandraInfo;
        private readonly IStatisticsDao _statsDao;

        public HomeController(ICassandraInfo cassandraInfo, IStatisticsDao statsDao)
        {
            if (cassandraInfo == null) throw new ArgumentNullException("cassandraInfo");
            if (statsDao == null) throw new ArgumentNullException("statsDao");
            _cassandraInfo = cassandraInfo;
            _statsDao = statsDao;
        }

        /// <summary>
        /// Displays the Playlist home page.
        /// </summary>
        public ActionResult Index()
        {
            _statsDao.IncrementCounter("page hits: home");

            return View(new IndexModel
            {
                CassandraClusterName = _cassandraInfo.GetClusterName(),
                CassandraVersion = _cassandraInfo.GetCassandraVersion(),
                ClrVersion = Environment.Version.ToString()
            });
        }
    }
}
