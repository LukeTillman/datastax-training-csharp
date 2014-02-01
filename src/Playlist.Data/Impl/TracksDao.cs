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
            return results.GetRows().Select(r => MapRowToTrackDto(r)).ToList();
        }

        /// <summary>
        /// Gets a list of songs by genre.
        /// </summary>
        public IEnumerable<TrackDto> ListSongsByGenre(string genre, int numTracks)
        {
            PreparedStatement preparedStatement = _session.Prepare("SELECT * FROM track_by_genre WHERE genre = ? LIMIT ?");
            BoundStatement boundStatement = preparedStatement.Bind(genre, numTracks);
            RowSet results = _session.Execute(boundStatement);
            return results.GetRows().Select(r => MapRowToTrackDto(r)).ToList();
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
            // and since the track_by_id table doesn't include the "starred" field, don't try to map it
            return MapRowToTrackDto(results.GetRows().SingleOrDefault(), includeStarred: false);
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

            PreparedStatement preparedStatement =
                _session.Prepare("UPDATE track_by_genre USING TTL 30 SET starred = ? WHERE genre = ? AND artist = ? AND track = ? AND track_id = ?");
            BoundStatement boundStatement = preparedStatement.Bind(true, dto.Genre, dto.Artist, dto.Track, dto.TrackId);
            _session.Execute(boundStatement);

            preparedStatement = _session.Prepare("UPDATE track_by_artist USING TTL 30 SET starred = ? WHERE artist = ? AND track = ? AND track_id = ?");
            boundStatement = preparedStatement.Bind(true, dto.Artist, dto.Track, dto.TrackId);
            _session.Execute(boundStatement);
        }

        /// <summary>
        /// Maps a Cassandra row to a TrackDto.  Could be replaced with something like AutoMapper.
        /// </summary>
        private static TrackDto MapRowToTrackDto(Row row, bool includeStarred = true)
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

            if (includeStarred == false)
                return dto;

            // If the field doesn't exist or is null we set it to false (this is WAY more efficient than wrapping a
            // GetValue<bool> in a try...catch because we don't incur the Exception overhead)
            var starred = row.GetValue<bool?>("starred");
            dto.Starred = starred ?? false;
            return dto;
        }
    }
}
