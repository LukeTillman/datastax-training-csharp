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
            PreparedStatement preparedStatement = _session.Prepare("SELECT * FROM track_by_artist WHERE artist = ?");
            BoundStatement boundStatement = preparedStatement.Bind(artist);
            RowSet results = _session.Execute(boundStatement);
            return results.GetRows().Select(MapRowToTrackDto).ToList();
        }

        /// <summary>
        /// Gets a list of songs by genre.
        /// </summary>
        public IEnumerable<TrackDto> ListSongsByGenre(string genre, int numTracks)
        {
            // TODO - The numTracks parameter contains the number of rows to return

            PreparedStatement preparedStatement = _session.Prepare("SELECT * FROM track_by_genre WHERE genre = ?");
            BoundStatement boundStatement = preparedStatement.Bind(genre);
            RowSet results = _session.Execute(boundStatement);

            return results.GetRows().Select(MapRowToTrackDto).ToList();
        }

        /// <summary>
        /// Gets a track by id or returns null if it cannot be found.
        /// </summary>
        public TrackDto GetTrackById(Guid trackId)
        {
            PreparedStatement preparedStatement = _session.Prepare("SELECT * FROM track_by_id WHERE track_id = ?");
            BoundStatement boundStatement = preparedStatement.Bind(trackId);
            RowSet results = _session.Execute(boundStatement);

            // Return null if there is no track found (map function will return null if row from SingleOrDefault is null)
            return MapRowToTrackDto(results.GetRows().SingleOrDefault());
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
                _session.Prepare("INSERT INTO track_by_id (genre, track_id, artist, track, track_length_in_seconds) VALUES (?, ?, ?, ?, ?)");
            boundStatement = preparedStatement.Bind(track.Genre, track.TrackId, track.Artist, track.Track, track.LengthInSeconds);
            _session.Execute(boundStatement);

            preparedStatement =
                _session.Prepare("INSERT INTO track_by_genre (genre, track_id, artist, track, track_length_in_seconds) VALUES (?, ?, ?, ?, ?)");
            boundStatement = preparedStatement.Bind(track.Genre, track.TrackId, track.Artist, track.Track, track.LengthInSeconds);
            _session.Execute(boundStatement);

            preparedStatement =
                _session.Prepare("INSERT INTO track_by_artist (genre, track_id, artist, track, track_length_in_seconds) VALUES (?, ?, ?, ?, ?)");
            boundStatement = preparedStatement.Bind(track.Genre, track.TrackId, track.Artist, track.Track, track.LengthInSeconds);
            _session.Execute(boundStatement);
        }

        /// <summary>
        /// Sets a track as being starred.
        /// </summary>
        public void Star(TrackDto dto)
        {
            if (dto == null) throw new ArgumentNullException("dto");

            // TODO - Implement the code to update the necessary tables to indicate that a row has been starred
        }

        /// <summary>
        /// Maps a Cassandra row to a TrackDto.  Could be replaced with something like AutoMapper.
        /// </summary>
        private static TrackDto MapRowToTrackDto(Row row)
        {
            if (row == null) return null;

            var dto = new TrackDto
            {
                TrackId = row.GetValue<Guid>("track_id"),
                Artist = row.GetValue<string>("artist"),
                Track = row.GetValue<string>("track"),
                Genre = row.GetValue<string>("genre"),
                MusicFile = row.GetValue<string>("music_file"),
                LengthInSeconds = row.GetValue<int>("track_length_in_seconds")
            };

            // TODO - modify this to set this to the value of the new boolean column
            dto.Starred = false;

            return dto;
        }
    }
}
