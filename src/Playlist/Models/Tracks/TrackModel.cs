using System;

namespace Playlist.Models.Tracks
{
    /// <summary>
    /// Model used to represent a track.
    /// </summary>
    public class TrackModel
    {
        public Guid TrackId { get; set; }
        public string Track { get; set; }
        public string Genre { get; set; }
        public TimeSpan Length { get; set; }
    }
}