using System;
using System.Linq;
using System.Web.Mvc;
using Playlist.ActionFilters;
using Playlist.Data;
using Playlist.Data.Dtos;
using Playlist.Models.PlaylistTracks;

namespace Playlist.Controllers
{
    public class PlaylistTracksController : Controller
    {
        private readonly IPlaylistsDao _playlistsDao;
        private readonly ITracksDao _tracksDao;

        public PlaylistTracksController(IPlaylistsDao playlistsDao, ITracksDao tracksDao)
        {
            if (playlistsDao == null) throw new ArgumentNullException("playlistsDao");
            if (tracksDao == null) throw new ArgumentNullException("tracksDao");
            _playlistsDao = playlistsDao;
            _tracksDao = tracksDao;
        }

        /// <summary>
        /// Shows the list of tracks in a playlist view.
        /// </summary>
        [HttpGet]
        [RequiresLoggedInUser]
        public ActionResult ListTracks(string playlistName)
        {
            if (playlistName == null) throw new ArgumentNullException("playlistName");

            var user = (UserDto) Session["user"];
            PlaylistDto playlist = _playlistsDao.GetPlaylistForUser(user.Username, playlistName);
            return View(new ListTracksModel
            {
                PlaylistName = playlist.PlaylistName,
                Username = playlist.Username,
                PlaylistLength = TimeSpan.FromSeconds(playlist.PlaylistLengthInSeconds),
                PlaylistTracks = playlist.PlaylistTrackList.Select(t => new PlaylistTrackModel
                {
                    SequenceNumber = t.SequenceNumber,
                    TrackName = t.TrackName,
                    Artist = t.Artist,
                    Genre = t.Genre,
                    Length = TimeSpan.FromSeconds(t.TrackLengthInSeconds)
                }).ToList()
            });
        }

        /// <summary>
        /// Adds a track to a playlist.
        /// </summary>
        [HttpPost]
        [RequiresLoggedInUser]
        public ActionResult AddTrackToPlaylist(AddTrackToPlaylistModel model)
        {
            // Grab the PlaylistTrack information from the DB
            TrackDto track = _tracksDao.GetTrackById(model.TrackId);
            var playlistTrack = new PlaylistTrackDto(track);

            var user = (UserDto)Session["user"];
            PlaylistDto playlist = _playlistsDao.GetPlaylistForUser(user.Username, model.PlaylistName);
            _playlistsDao.AddTrackToPlaylist(playlist, playlistTrack);

            return RedirectToAction("ListTracks", "PlaylistTracks", new {playlistName = model.PlaylistName});
        }

        /// <summary>
        /// Removes a track from a playlist.
        /// </summary>
        [HttpGet]
        [RequiresLoggedInUser]
        public ActionResult DeleteTrackFromPlaylist(DeleteTrackFromPlaylistModel model)
        {
            var user = (UserDto) Session["user"];
            PlaylistDto playlist = _playlistsDao.GetPlaylistForUser(user.Username, model.PlaylistName);
            _playlistsDao.DeleteTrackFromPlaylist(playlist, model.SequenceNumber);
            return RedirectToAction("ListTracks", "PlaylistTracks", new {playlistName = model.PlaylistName});
        }
    }
}
