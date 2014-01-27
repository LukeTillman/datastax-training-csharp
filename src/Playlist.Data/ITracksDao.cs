using System;
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
        IEnumerable<TrackDto> ListSongsByGenre(string genre, int numTracks);

        /// <summary>
        /// Gets a track by id or returns null if it cannot be found.
        /// </summary>
        TrackDto GetTrackById(Guid trackId);

        /// <summary>
        /// Add a track to the database
        /// </summary>
        void Add(TrackDto track);

        /// <summary>
        /// Sets a track as being starred.
        /// </summary>
        void Star(TrackDto dto);
    }
}