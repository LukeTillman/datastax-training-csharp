using System.Collections.Generic;

namespace Playlist.Data
{
    public interface IArtistsDao
    {
        /// <summary>
        /// Returns a list of artists that begin with the specified letter.  The artist
        /// may be returned in ascending or descending order.
        /// </summary>
        IEnumerable<string> ListArtistsByLetter(string firstLetter, bool desc);
    }
}