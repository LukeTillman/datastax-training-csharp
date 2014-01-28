using Playlist.Data.Dtos;

namespace Playlist.Data
{
    public interface IPlaylistsDao
    {
        /// <summary>
        /// Creates a new playlist for the user specified and returns it.
        /// </summary>
        PlaylistDto CreatePlaylist(UserDto user, string playlistName);

        /// <summary>
        /// Deletes the playlist specified.
        /// </summary>
        void DeletePlaylist(UserDto user, string playlistName);
    }
}