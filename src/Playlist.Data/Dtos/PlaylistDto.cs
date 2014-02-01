using System.Collections.Generic;

namespace Playlist.Data.Dtos
{
    /// <summary>
    /// A playlist in the database.
    /// </summary>
    public class PlaylistDto
    {
        internal readonly List<PlaylistTrackDto> TrackList;

        public string Username { get; internal set; }
        public string PlaylistName { get; internal set; }
        public int PlaylistLengthInSeconds { get; internal set; }

        public IEnumerable<PlaylistTrackDto> PlaylistTrackList
        {
            get { return TrackList; }
        }

        internal PlaylistDto()
        {
            TrackList = new List<PlaylistTrackDto>();
        }
    }
}
