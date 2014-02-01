using System;

namespace Playlist.Models.PlaylistTracks
{
    /// <summary>
    /// Model needed to add a track to a playlist.
    /// </summary>
    public class AddTrackToPlaylistModel
    {
        public string PlaylistName { get; set; }
        public Guid TrackId { get; set; }
    }
}