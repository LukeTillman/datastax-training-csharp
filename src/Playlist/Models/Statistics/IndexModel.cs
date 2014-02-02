using System.Collections.Generic;

namespace Playlist.Models.Statistics
{
    /// <summary>
    /// Model for viewing site statistics.
    /// </summary>
    public class IndexModel
    {
        public IEnumerable<StatCounterModel> Statistics { get; set; }
    }
}