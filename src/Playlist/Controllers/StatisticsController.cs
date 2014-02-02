using System;
using System.Linq;
using System.Web.Mvc;
using Playlist.Data;
using Playlist.Models.Statistics;

namespace Playlist.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly IStatisticsDao _statsDao;

        public StatisticsController(IStatisticsDao statsDao)
        {
            if (statsDao == null) throw new ArgumentNullException("statsDao");
            _statsDao = statsDao;
        }

        /// <summary>
        /// Shows the Statistics view.
        /// </summary>
        public ActionResult Index()
        {
            var model = new IndexModel
            {
                Statistics = _statsDao.GetStatistics().Select(s => new StatCounterModel
                {
                    CounterName = s.CounterName,
                    CounterValue = s.CounterValue
                }).ToList()
            };
            return View(model);
        }
    }
}
