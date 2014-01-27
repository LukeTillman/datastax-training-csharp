using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra;
using Playlist.Data.Dtos;

namespace Playlist.Data.Impl
{
    /// <summary>
    /// Tracks data access using Cassandra.
    /// </summary>
    public class TracksDao : ITracksDao
    {
        private readonly Session _session;

        public TracksDao(Session session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _session = session;
        }

        /// <summary>
        /// Gets a list of songs by artist.
        /// </summary>
        public IEnumerable<TrackDto> ListSongsByArtist(string artist)
        {
            // TODO - This bombs if the artist name contains a single-quote.
            // TODO - Replace the next two lines of this method
            // TODO - with code which uses a prepared statement and bound statement

            string queryText = string.Format("SELECT * FROM track_by_artist WHERE artist = '{0}'", artist);
            RowSet results = _session.Execute(queryText);

            // TODO - Done replacing code

            return results.GetRows().Select(MapRowToTrackDto).ToList();
        }

        /// <summary>
        /// Gets a list of songs by genre.
        /// </summary>
        public IEnumerable<TrackDto> ListSongsByGenre(string genre)
        {
            RowSet results = null;
            
            // TODO - implement the code here to retrieve the songs by genre in the "results" variable
            if (results == null)
                return new List<TrackDto>();

            return results.GetRows().Select(MapRowToTrackDto).ToList();
        }

        /// <summary>
        /// Add a track to the database
        /// </summary>
        public void Add(TrackDto track)
        {
            if (track == null) throw new ArgumentNullException("track");

            // Compute the first letter of the artists name for the artists_by_first_letter table
            string artistFirstLetter = track.Artist.Substring(0, 1).ToUpper();

            PreparedStatement preparedStatement = _session.Prepare("INSERT INTO artists_by_first_letter (first_letter, artist) VALUES (?, ?)");
            BoundStatement boundStatement = preparedStatement.Bind(artistFirstLetter, track.Artist);
            _session.Execute(boundStatement);

            preparedStatement =
                _session.Prepare("INSERT INTO track_by_artist (genre, track_id, artist, track, track_length_in_seconds) VALUES (?, ?, ?, ?, ?)");
            boundStatement = preparedStatement.Bind(track.Genre, track.TrackId, track.Artist, track.Track, track.LengthInSeconds);
            _session.Execute(boundStatement);

            // TODO - what are we missing here?
        }

        /// <summary>
        /// Maps a Cassandra row to a TrackDto.  Could be replaced with something like AutoMapper.
        /// </summary>
        private static TrackDto MapRowToTrackDto(Row row)
        {
            return new TrackDto
            {
                TrackId = row.GetValue<Guid>("track_id"),
                Artist = row.GetValue<string>("artist"),
                Track = row.GetValue<string>("track"),
                Genre = row.GetValue<string>("genre"),
                MusicFile = row.GetValue<string>("music_file"),
                LengthInSeconds = row.GetValue<int>("track_length_in_seconds")
            };
        }
    }
}
