using System;

namespace Playlist.Models.Tracks
{
    /// <summary>
    /// The model used to star a track.
    /// </summary>
    public class StarTrackModel
    {
        public Guid TrackId { get; set; }
        public string ReturnUrl { get; set; }
    }
}