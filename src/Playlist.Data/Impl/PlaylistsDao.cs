using System;
using Cassandra;
using Cassandra.Data.Linq;
using Playlist.Data.Dtos;

namespace Playlist.Data.Impl
{
    /// <summary>
    /// Playlists data access using Cassandra.
    /// </summary>
    public class PlaylistsDao : IPlaylistsDao
    {
        private readonly Session _session;

        public PlaylistsDao(Session session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _session = session;
        }

        /// <summary>
        /// Creates a new playlist for the user specified and returns it.
        /// </summary>
        public PlaylistDto CreatePlaylist(UserDto user, string playlistName)
        {
            // Change single quotes to a pair of single quotes for escaping into the database
            string fixedPlaylistName = playlistName.Replace("'", "''");

            PreparedStatement prepared =
                _session.Prepare(string.Format("UPDATE users SET playlist_names = playlist_names + {{'{0}'}} WHERE username = ?", fixedPlaylistName));
            BoundStatement bound = prepared.Bind(user.Username);
            _session.Execute(bound);

            // Update the user object too
            user.PlaylistNames.Add(playlistName);

            return new PlaylistDto
            {
                Username = user.Username,
                PlaylistName = playlistName
            };
        }

        /// <summary>
        /// Deletes the playlist specified.
        /// </summary>
        public void DeletePlaylist(UserDto user, string playlistName)
        {
            // Change single quotes to a pair of single quotes for escaping into the database
            string fixedPlaylistName = playlistName.Replace("'", "''");

            const string batchStatement = "BEGIN BATCH " +
                                          "UPDATE users SET playlist_names = playlist_names - {{ '{0}' }} WHERE username = ? " +
                                          "DELETE FROM playlist_tracks WHERE username = ? AND playlist_name = ? " +
                                          "APPLY BATCH";
            
            PreparedStatement prepared = _session.Prepare(string.Format(batchStatement, fixedPlaylistName));
            BoundStatement bound = prepared.Bind(user.Username, user.Username, playlistName);
            _session.Execute(bound);

            // Update the user object too
            user.PlaylistNames.Remove(playlistName);
        }

        /// <summary>
        /// Gets the playlist specified for the user specified.
        /// </summary>
        public PlaylistDto GetPlaylistForUser(string username, string playlistName)
        {
            // Create a new empty playlist object
            var playlist = new PlaylistDto {Username = username, PlaylistName = playlistName};

            // Read the tracks from the database
            PreparedStatement prepared =
                _session.Prepare("SELECT username, playlist_name, sequence_no, artist, track_name, track_id, genre, track_length_in_seconds " +
                                 "FROM playlist_tracks WHERE username = ? and playlist_name = ?");
            BoundStatement bound = prepared.Bind(username, playlistName);
            RowSet results = _session.Execute(bound);

            foreach(Row row in results.GetRows())
            {
                PlaylistTrackDto track = MapRowToPlaylistTrack(row);
                playlist.TrackList.Add(track);

                // Pre-aggregate the playlist length in seconds;
                playlist.PlaylistLengthInSeconds += track.TrackLengthInSeconds;
            }
            
            return playlist;
        }

        /// <summary>
        /// Deletes a track from a playlist by its sequence number.
        /// </summary>
        public void DeleteTrackFromPlaylist(PlaylistDto playlist, long sequenceNumber)
        {
            // Find the track to delete, and delete it from the list

            // Find the track that has a matching sequence number
            int index = playlist.TrackList.FindIndex(track => track.SequenceNumber == sequenceNumber);

            // If the track was not found nothing to do
            if (index < 0)
                return;
            
            // Get the track to delete, remove it from the playlist's track list
            PlaylistTrackDto playlistTrackToDelete = playlist.TrackList[index];
            playlist.TrackList.RemoveAt(index);

            // Adjust the playlist length
            playlist.PlaylistLengthInSeconds -= playlistTrackToDelete.TrackLengthInSeconds;

            // Remove it from the database
            PreparedStatement prepared = _session.Prepare("DELETE from playlist_tracks where username = ? and playlist_name = ? and sequence_no = ?");
            BoundStatement bound = prepared.Bind(playlist.Username, playlist.PlaylistName,
                                                 SequenceToDateTimeOffset(playlistTrackToDelete.SequenceNumber));
            _session.Execute(bound);
        }

        /// <summary>
        /// Adds a track to a playlist.
        /// </summary>
        public void AddTrackToPlaylist(PlaylistDto playlist, PlaylistTrackDto playlistTrack)
        {
            PreparedStatement prepared = _session.Prepare("INSERT into playlist_tracks" +
                                                          " (username, playlist_name, sequence_no, artist, track_name, genre, track_id, track_length_in_seconds) " +
                                                          "VALUES (?, ?, ?, ?, ?, ?, ?, ?)");
            
            // Since the playlistTrack sequence is like a time-series, set it's sequence to the current time
            // Also update the total time for the playlist locally.
            playlistTrack.SequenceNumber = DateTimeOffsetToSequence(DateTimeOffset.Now);
            playlist.PlaylistLengthInSeconds += playlistTrack.TrackLengthInSeconds;

            // TODO:  If the C# driver adds support for binding parameters by name, use that here
            BoundStatement bound = prepared.Bind(playlist.Username, playlist.PlaylistName,
                                                 SequenceToDateTimeOffset(playlistTrack.SequenceNumber), playlistTrack.Artist,
                                                 playlistTrack.TrackName, playlistTrack.Genre, playlistTrack.TrackId,
                                                 playlistTrack.TrackLengthInSeconds);

            _session.Execute(bound);

            playlist.TrackList.Add(playlistTrack);
        }

        /// <summary>
        /// Maps a Cassandra row to a PlaylistTrackDto.
        /// </summary>
        private static PlaylistTrackDto MapRowToPlaylistTrack(Row row)
        {
            if (row == null) return null;

            return new PlaylistTrackDto
            {
                TrackName = row.GetValue<string>("track_name"),
                Artist = row.GetValue<string>("artist"),
                TrackLengthInSeconds = row.GetValue<int>("track_length_in_seconds"),
                SequenceNumber = DateTimeOffsetToSequence(row.GetValue<DateTimeOffset>("sequence_no")),
                TrackId = row.GetValue<Guid>("track_id"),
                Genre = row.GetValue<string>("genre")
            };
        }

        /// <summary>
        /// Converts the DateTimeOffset specified to a sequence number for use in the application.
        /// </summary>
        private static long DateTimeOffsetToSequence(DateTimeOffset dateTime)
        {
            // Use UTC ticks as the sequence number
            return dateTime.ToUniversalTime().Ticks;
        }

        /// <summary>
        /// Converts the sequence number specified to a DateTimeOffset for use with the sequence_no timestamp column in Cassandra.
        /// </summary>
        private static DateTimeOffset SequenceToDateTimeOffset(long sequenceNumber)
        {
            // Since we're using UTC ticks when generating a sequence number, we can use TimeSpan.Zero here
            return new DateTimeOffset(sequenceNumber, TimeSpan.Zero);
        }
    }
}
