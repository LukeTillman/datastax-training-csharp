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
        public ActionResult Index(string artist, string genre, int? howMany, bool? frame)
        {
            // If howMany is null, default it to 25
            if (howMany.HasValue == false) howMany = 25;

            IEnumerable<TrackDto> tracks;
            if (string.IsNullOrEmpty(artist) == false)
            {
                // Assume we're searching by artist
                tracks = _tracksDao.ListSongsByArtist(artist);
            }
            else if (string.IsNullOrEmpty(genre) == false)
            {
                // If howMany is 0, assume that means "All" and default to 100k
                int numTracks = howMany.Value;
                if (numTracks == 0)
                    numTracks = 100000;

                // Assume we're searching by genre
                tracks = _tracksDao.ListSongsByGenre(genre, numTracks);
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
                    Length = TimeSpan.FromSeconds(dto.LengthInSeconds),
                    Starred = dto.Starred
                }),
                HowMany = howMany.Value,
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

        /// <summary>
        /// Stars a track.
        /// </summary>
        [HttpPost]
        public ActionResult StarTrack(StarTrackModel model)
        {
            TrackDto dbTrack = _tracksDao.GetTrackById(model.TrackId);
            _tracksDao.Star(dbTrack);
            return Redirect(model.ReturnUrl);
        }
    }
}
