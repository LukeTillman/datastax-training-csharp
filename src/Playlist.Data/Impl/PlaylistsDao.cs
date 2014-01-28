using System;
using Cassandra;
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
            String fixedPlaylistName = playlistName.Replace("'", "''");

            // TODO
            // TODO - Add the playlist name to the playlist_names Set
            // TODO - Hint: insert the fixed_playlist_name value into the set using the special CQL syntax for mutating sets.
            // TODO

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

            // TODO
            // TODO - Add code here to delete this playlist from the database
            // TODO - Hint: This code is similar to the the addPlayList method
            // TODO
        }
    }
}
