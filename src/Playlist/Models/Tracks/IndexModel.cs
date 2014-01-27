using System.Collections.Generic;

namespace Playlist.Models.Tracks
{
    /// <summary>
    /// The model for the Tracks view.
    /// </summary>
    public class IndexModel
    {
        public string Artist { get; set; }
        public string Genre { get; set; }
        public IEnumerable<TrackModel> Tracks { get; set; }
        public int HowMany { get; set; }
        public bool? Frame { get; set; }
    }
}