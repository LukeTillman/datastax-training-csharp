using System;

namespace Playlist.Data.Dtos
{
    /// <summary>
    /// Represents a Track in the database.
    /// </summary>
    public class TrackDto
    {
        public Guid TrackId { get; internal set; }
        public string Artist { get; internal set; }
        public string Track { get; internal set; }
        public string Genre { get; internal set; }
        public string MusicFile { get; internal set; }
        public int LengthInSeconds { get; internal set; }

        /// <summary>
        /// Internal-only constructor for use when pulling data from the database.
        /// </summary>
        internal TrackDto()
        {
        }

        /// <summary>
        /// Creates a new Track DTO for addition to the database.
        /// </summary>
        public TrackDto(string artist, string track, string genre, string musicFile, int lengthInSeconds)
        {
            // We can generate the new UUID right here in the constructor
            TrackId = Guid.NewGuid();
            Artist = artist;
            Track = track;
            Genre = genre;
            MusicFile = musicFile;
            LengthInSeconds = lengthInSeconds;
        }
    }
}