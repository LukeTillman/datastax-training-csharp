using System;
using System.Linq;
using Cassandra;

namespace Playlist.Data.Impl
{
    /// <summary>
    /// Gets Cassandra cluster information.
    /// </summary>
    public class CassandraInfo : ICassandraInfo
    {
        private readonly string _clusterName;
        private readonly string _cassandraVersion;

        public CassandraInfo(Session session)
        {
            if (session == null) throw new ArgumentNullException("session");

            // I don't love having a constructor executing a query, but that's how the Java sample does it, so here we go...
            Row row = session.Execute("select cluster_name, release_version from system.local").GetRows().Single();
            _clusterName = row.GetValue<string>("cluster_name");
            _cassandraVersion = row.GetValue<string>("release_version");
        }

        /// <summary>
        /// Gets the Cassandra cluster name.
        /// </summary>
        public string GetClusterName()
        {
            return _clusterName;
        }

        /// <summary>
        /// Gets the Cassandra version for the cluster.
        /// </summary>
        public string GetCassandraVersion()
        {
            return _cassandraVersion;
        }
    }
}
