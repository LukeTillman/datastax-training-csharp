using Playlist.Data.Dtos;

namespace Playlist.Data
{
    public interface IUsersDao
    {
        /// <summary>
        /// Adds a new user and returns the user.  Returns null if the user cannot be created because that username
        /// already exists.
        /// </summary>
        UserDto AddUser(string username, string password);

        /// <summary>
        /// Delete the user.  It does not need to check if the user already exists.
        /// </summary>
        void DeleteUser(UserDto user);

        /// <summary>
        /// Looks up a user by email address (username).
        /// </summary>
        UserDto GetUser(string username);

        /// <summary>
        /// The also retrieves a user based on the username, but also validates its password.  Returns null if the
        /// username or password is incorrect.
        /// </summary>
        UserDto ValidateLogin(string username, string password);
    }
}