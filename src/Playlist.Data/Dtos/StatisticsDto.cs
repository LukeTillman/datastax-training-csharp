namespace Playlist.Data.Dtos
{
    /// <summary>
    /// Represents statistics in the database.
    /// </summary>
    public class StatisticsDto
    {
        public string CounterName { get; internal set; }
        public long CounterValue { get; internal set; }
    }
}
