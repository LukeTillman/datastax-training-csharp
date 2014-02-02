using System.Collections.Generic;
using Playlist.Data.Dtos;

namespace Playlist.Data
{
    public interface IStatisticsDao
    {
        /// <summary>
        /// Retrieves the statistics.
        /// </summary>
        IEnumerable<StatisticsDto> GetStatistics();

        /// <summary>
        /// Increments a statistics counter.
        /// </summary>
        void IncrementCounter(string counterName);

        /// <summary>
        /// Decrements a statistics counter.
        /// </summary>
        void DecrementCounter(string counterName);
    }
}