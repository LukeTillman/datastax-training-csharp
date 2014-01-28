using System;
using System.Web.Mvc;
using Playlist.ActionFilters;
using Playlist.Data;
using Playlist.Data.Dtos;
using Playlist.Models.Playlists;


namespace Playlist.Controllers
{
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistsDao _playlistsDao;

        public PlaylistsController(IPlaylistsDao playlistsDao)
        {
            if (playlistsDao == null) throw new ArgumentNullException("playlistsDao");
            _playlistsDao = playlistsDao;
        }

        /// <summary>
        /// Shows the playlists view for the currently logged in user.
        /// </summary>
        [ExportModelStateToTempData]
        public ActionResult Index()
        {
            // Get logged in user and if not logged in, redirect to login page
            var user = (UserDto) Session["user"];
            if (user == null)
            {
                ModelState.AddModelError("", "Not logged in.");
                return RedirectToAction("Index", "Login");
            }

            return View(new IndexModel
            {
                Username = user.Username,
                PlaylistNames = user.PlaylistNames
            });
        }

        /// <summary>
        /// Adds a new playlist for the current user.
        /// </summary>
        [HttpPost]
        public ActionResult AddPlaylist(AddPlaylistModel model)
        {
            // Get logged in user and if not logged in, redirect to login page
            var user = (UserDto)Session["user"];
            if (user == null)
            {
                ModelState.AddModelError("", "Not logged in.");
                return RedirectToAction("Index", "Login");
            }

            _playlistsDao.CreatePlaylist(user, model.Playlist);
            return RedirectToAction("Index", "Playlists");
        }

        /// <summary>
        /// Deletes a playlist for the current user.
        /// </summary>
        [HttpGet]
        public ActionResult DeletePlaylist(AddPlaylistModel model)
        {
            // Get logged in user and if not logged in, redirect to login page
            var user = (UserDto)Session["user"];
            if (user == null)
            {
                ModelState.AddModelError("", "Not logged in.");
                return RedirectToAction("Index", "Login");
            }

            _playlistsDao.DeletePlaylist(user, model.Playlist);
            return RedirectToAction("Index", "Playlists");
        }
    }
}
