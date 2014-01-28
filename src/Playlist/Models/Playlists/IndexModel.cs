using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Playlist.Models.Playlists
{
    /// <summary>
    /// The model used to view the playlists page.
    /// </summary>
    public class IndexModel
    {
        public string Username { get; set; }
        public IEnumerable<string> PlaylistNames { get; set; } 
    }
}