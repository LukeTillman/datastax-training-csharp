using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra;

namespace Playlist.Data.Impl
{
    /// <summary>
    /// Artists data access using Cassandra.
    /// </summary>
    public class ArtistsDao : IArtistsDao
    {
        private readonly Session _session;

        public ArtistsDao(Session session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _session = session;
        }

        /// <summary>
        /// Returns a list of artists that begin with the specified letter.  The artist
        /// may be returned in ascending or descending order.
        /// </summary>
        public IEnumerable<string> ListArtistsByLetter(string firstLetter, bool desc)
        {
            // Build and execute the query
            string queryText = string.Format("SELECT * FROM artists_by_first_letter WHERE first_letter = '{0}'", firstLetter);
            RowSet results = _session.Execute(queryText);

            // Iterate the resulting rows and select just the value for the "artist" column into a return List
            return results.GetRows().Select(row => row.GetValue<string>("artist")).ToList();
        }
    }
}
