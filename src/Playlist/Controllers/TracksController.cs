using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Playlist.ActionFilters;
using Playlist.Data;
using Playlist.Data.Dtos;
using Playlist.Models.Tracks;

namespace Playlist.Controllers
{
    public class TracksController : Controller
    {
        private readonly ITracksDao _tracksDao;

        public TracksController(ITracksDao tracksDao)
        {
            if (tracksDao == null) throw new ArgumentNullException("tracksDao");
            _tracksDao = tracksDao;
        }

        /// <summary>
        /// Shows the Tracks view (optionally by artist or genre specified).
        /// </summary>
        [HttpGet]
        public ActionResult Index(string artist, string genre, bool? frame)
        {
            IEnumerable<TrackDto> tracks;
            if (string.IsNullOrEmpty(artist) == false)
            {
                // Assume we're searching by artist
                tracks = _tracksDao.ListSongsByArtist(artist);
            }
            else if (string.IsNullOrEmpty(genre) == false)
            {
                // Assume we're searching by genre
                tracks = _tracksDao.ListSongsByGenre(genre);
            }
            else
            {
                tracks = new List<TrackDto>();
            }

            var model = new IndexModel
            {
                Artist = artist,
                Genre = genre,
                Tracks = tracks.Select(dto => new TrackModel
                {
                    TrackId = dto.TrackId, 
                    Track = dto.Track,
                    Genre = dto.Genre,
                    Length = TimeSpan.FromSeconds(dto.LengthInSeconds)
                }),
                Frame = frame
            };
            return View(model);
        }

        /// <summary>
        /// Shows the view for adding a new track.
        /// </summary>
        [HttpGet]
        [ImportModelStateFromTempData]
        public ActionResult AddTrack()
        {
            return View(new AddTrackModel());
        }

        /// <summary>
        /// Handles submit of the add new track form.
        /// </summary>
        [HttpPost]
        [ExportModelStateToTempData]
        public ActionResult AddTrackSubmit(AddTrackModel model)
        {
            // If not valid, redirect back to the AddTrack view (the ExportModelStateToTempData attribute will pass any ModelState errors)
            if (ModelState.IsValid == false)
                return RedirectToAction("AddTrack");

            // Add the new track
            var dbDto = new TrackDto(model.Artist, model.Track, model.Genre, model.MusicFile, model.LengthInSeconds);
            _tracksDao.Add(dbDto);

            // Go to the artist to see the new track
            return RedirectToAction("Index", "Tracks", new {artist = model.Artist});
        }
    }
}
