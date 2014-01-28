namespace Playlist.Data.Dtos
{
    /// <summary>
    /// A playlist in the database.
    /// </summary>
    public class PlaylistDto
    {
        public string Username { get; internal set; }
        public string PlaylistName { get; internal set; }

        internal PlaylistDto()
        {
        }
    }
}
