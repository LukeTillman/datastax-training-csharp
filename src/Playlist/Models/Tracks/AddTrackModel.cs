namespace Playlist.Models.Tracks
{
    /// <summary>
    /// The view model for adding a new track.
    /// </summary>
    public class AddTrackModel
    {
        public string Artist { get; set; }
        public string Track { get; set; }
        public string Genre { get; set; }
        public string MusicFile { get; set; }
        public int LengthInSeconds { get; set; }
    }
}