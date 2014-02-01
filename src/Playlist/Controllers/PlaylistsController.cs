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
        [RequiresLoggedInUser]
        public ActionResult Index()
        {
            var user = (UserDto) Session["user"];
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
        [RequiresLoggedInUser]
        public ActionResult AddPlaylist(AddPlaylistModel model)
        {
            // Get logged in user and if not logged in, redirect to login page
            var user = (UserDto) Session["user"];
            _playlistsDao.CreatePlaylist(user, model.Playlist);
            return RedirectToAction("Index", "Playlists");
        }

        /// <summary>
        /// Deletes a playlist for the current user.
        /// </summary>
        [HttpGet]
        [RequiresLoggedInUser]
        public ActionResult DeletePlaylist(AddPlaylistModel model)
        {
            // Get logged in user and if not logged in, redirect to login page
            var user = (UserDto) Session["user"];
            _playlistsDao.DeletePlaylist(user, model.Playlist);
            return RedirectToAction("Index", "Playlists");
        }
    }
}
