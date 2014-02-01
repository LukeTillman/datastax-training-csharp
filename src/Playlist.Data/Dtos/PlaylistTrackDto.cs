using System;

namespace Playlist.Data.Dtos
{
    /// <summary>
    /// A track in a playlist in the database.
    /// </summary>
    public class PlaylistTrackDto
    {
        public string TrackName { get; internal set; }
        public string Artist { get; internal set; }
        public int TrackLengthInSeconds { get; internal set; }
        public string Genre { get; internal set; }
        public Guid TrackId { get; internal set; }
        public long SequenceNumber { get; internal set; }

        internal PlaylistTrackDto()
        {
        }

        /// <summary>
        /// Creates a PlaylistTrackDto from a track DTO.  Used when adding a new track to a playlist.
        /// </summary>
        public PlaylistTrackDto(TrackDto track)
        {
            TrackName = track.Track;
            Artist = track.Artist;
            TrackLengthInSeconds = track.LengthInSeconds;
            Genre = track.Genre;
            TrackId = track.TrackId;
            // Sequence number will be managed by the DAO
        }
    }
}