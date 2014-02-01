namespace Playlist.Models.PlaylistTracks
{
    /// <summary>
    /// Model for deleting a track from a playlist.
    /// </summary>
    public class DeleteTrackFromPlaylistModel
    {
        public string PlaylistName { get; set; }
        public long SequenceNumber { get; set; }
    }
}