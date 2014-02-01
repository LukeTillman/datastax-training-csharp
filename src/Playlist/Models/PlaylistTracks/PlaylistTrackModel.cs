using System;

namespace Playlist.Models.PlaylistTracks
{
    /// <summary>
    /// Model for a track in a playlist.
    /// </summary>
    public class PlaylistTrackModel
    {
        public long SequenceNumber { get; set; }
        public string TrackName { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public TimeSpan Length { get; set; }
    }
}