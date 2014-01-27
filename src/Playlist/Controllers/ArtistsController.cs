using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Playlist.Data;
using Playlist.Models.Artists;

namespace Playlist.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly IArtistsDao _artistDao;

        public ArtistsController(IArtistsDao artistDao)
        {
            if (artistDao == null) throw new ArgumentNullException("artistDao");
            _artistDao = artistDao;
        }

        [HttpGet]
        public ActionResult Index(string q, string order, bool? frame)
        {
            bool desc = order == "down";

            // Lookup artists by letter if a letter is specified
            IEnumerable<string> artists = string.IsNullOrEmpty(q)
                ? new List<string>()
                : _artistDao.ListArtistsByLetter(q, desc);

            return View(new IndexModel
            {
                FirstLetter = q,
                Artists = artists,
                Frame = frame
            });
        }
    }
}
