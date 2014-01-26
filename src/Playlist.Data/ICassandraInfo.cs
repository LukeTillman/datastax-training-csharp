namespace Playlist.Data
{
    /// <summary>
    /// Gets Cassandra cluster information.
    /// </summary>
    public interface ICassandraInfo
    {
        /// <summary>
        /// Gets the Cassandra cluster name.
        /// </summary>
        string GetClusterName();

        /// <summary>
        /// Gets the Cassandra version for the cluster.
        /// </summary>
        string GetCassandraVersion();
    }
}