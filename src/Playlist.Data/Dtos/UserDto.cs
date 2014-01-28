using System.Collections.Generic;

namespace Playlist.Data.Dtos
{
    /// <summary>
    /// A user in the database.
    /// </summary>
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public ISet<string> PlaylistNames { get; set; }

        internal UserDto()
        {
        }
    }
}
