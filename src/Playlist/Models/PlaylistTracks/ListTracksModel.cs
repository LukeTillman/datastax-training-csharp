using System;
using System.Collections.Generic;

namespace Playlist.Models.PlaylistTracks
{
    /// <summary>
    /// Model for listing tracks on a playlist
    /// </summary>
    public class ListTracksModel
    {
        public string PlaylistName { get; set; }
        public string Username { get; set; }
        public TimeSpan PlaylistLength { get; set; }
        public IEnumerable<PlaylistTrackModel> PlaylistTracks { get; set; }
    }
}