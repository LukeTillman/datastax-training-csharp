namespace Playlist.Models.Shared
{
    /// <summary>
    /// A model class with a couple static lists used in various views in the app.
    /// </summary>
    public static class ListsModel
    {
        /// <summary>
        /// The list of available Genres.
        /// </summary>
        public static string[] Genres =
        {
            "classic pop and rock", "classical", "dance and electronica", "folk", "hip-hop", "jazz and blues", "metal",
            "pop", "punk", "soul and reggae"
        };

        /// <summary>
        /// A list of letters, A-Z.
        /// </summary>
        public static string[] Letters =
        {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",
            "W", "X", "Y", "Z"
        };
    }
}