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

        /// <summary>
        /// Gets the playlist specified for the user specified.
        /// </summary>
        PlaylistDto GetPlaylistForUser(string username, string playlistName);

        /// <summary>
        /// Deletes a track from a playlist by its sequence number.
        /// </summary>
        void DeleteTrackFromPlaylist(PlaylistDto playlist, long sequenceNumber);

        /// <summary>
        /// Adds a track to a playlist.
        /// </summary>
        void AddTrackToPlaylist(PlaylistDto playlist, PlaylistTrackDto playlistTrack);
    }
}