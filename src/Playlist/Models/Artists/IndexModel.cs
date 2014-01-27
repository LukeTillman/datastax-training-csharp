using System.Collections.Generic;

namespace Playlist.Models.Artists
{
    /// <summary>
    /// Model for the Artists home page.
    /// </summary>
    public class IndexModel
    {
        public string FirstLetter { get; set; }
        public IEnumerable<string> Artists { get; set; }
        public bool? Frame { get; set; }
    }
}