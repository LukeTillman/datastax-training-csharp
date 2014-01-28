using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cassandra;
using Playlist.Data.Dtos;

namespace Playlist.Data.Impl
{
    /// <summary>
    /// Users data access using Cassandra.
    /// </summary>
    public class UsersDao : IUsersDao
    {
        private readonly Session _session;

        public UsersDao(Session session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _session = session;
        }

        /// <summary>
        /// Adds a new user and returns the user.  Returns null if the user cannot be created because that username
        /// already exists.
        /// </summary>
        public UserDto AddUser(string username, string password)
        {
            string queryText = "INSERT INTO users ... ";            // TODO - fill in the rest of this statement

            // TODO
            // TODO - prepare and execute the statement above to insert a new user. Also, capture the result set in a variable
            // TODO - hint - the code is something like this:  ResultSet result = <execute statement>
            // TODO


            bool userGotInserted = false;   // just initialize this so this compiles.

            // TODO - Retrieve the value of the "[applied]" column
            // TODO - hint its something like userGotInserted = result.<get me the first row>.<get the boolean value of the "[applied]" column>

            // Return null if the user was not inserted

            if (userGotInserted == false)
                return null;

            // Return the new user so the caller can get the userid
            return new UserDto()
            {
                Username = username,
                Password = password
            };
        }

        /// <summary>
        /// Delete the user.  It does not need to check if the user already exists.
        /// </summary>
        public void DeleteUser(UserDto user)
        {
            _session.Execute(string.Format("DELETE FROM users where username = '{0}'", user.Username));
        }

        /// <summary>
        /// Looks up a user by email address (username).
        /// </summary>
        public UserDto GetUser(string username)
        {
            string queryText = string.Format("SELECT * FROM users where username = '{0}'", username);
            RowSet results = _session.Execute(queryText);
            return MapRowToUserDto(results.GetRows().SingleOrDefault());
        }

        /// <summary>
        /// The also retrieves a user based on the username, but also validates its password.  Returns null if the
        /// username or password is incorrect.
        /// </summary>
        public UserDto ValidateLogin(string username, string password)
        {
            UserDto user = GetUser(username);
            if (user == null || user.Password != password)
                return null;

            return user;
        }

        /// <summary>
        /// Maps a Cassandra row to a UserDto.  Returns null if the row is null.
        /// </summary>
        private static UserDto MapRowToUserDto(Row row)
        {
            if (row == null) return null;

            return new UserDto
            {
                Username = row.GetValue<string>("username"),
                Password = row.GetValue<string>("password"),
                // We do this because we want a sorted set, and Cassandra only returns a regular IEnumerable<string>
                PlaylistNames = new SortedSet<string>(row.GetValue<IEnumerable<string>>("playlist_names") ?? new string[] {})
            };
        }
    }
}