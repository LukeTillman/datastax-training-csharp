using System.Collections.Generic;
using Playlist.Data.Dtos;

namespace Playlist.Data
{
    public interface ITracksDao
    {
        /// <summary>
        /// Gets a list of songs by artist.
        /// </summary>
        IEnumerable<TrackDto> ListSongsByArtist(string artist);

        /// <summary>
        /// Gets a list of songs by genre.
        /// </summary>
        IEnumerable<TrackDto> ListSongsByGenre(string genre);

        /// <summary>
        /// Add a track to the database
        /// </summary>
        void Add(TrackDto track);
    }
}