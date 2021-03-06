﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cassandra;
using Playlist.Data.Dtos;

namespace Playlist.Data.Impl
{
    /// <summary>
    /// Statistics data access with Cassandra.
    /// </summary>
    public class StatisticsDao : IStatisticsDao
    {
        private readonly Session _session;

        public StatisticsDao(Session session)
        {
            if (session == null) throw new ArgumentNullException("session");
            _session = session;
        }

        /// <summary>
        /// Retrieves the statistics.
        /// </summary>
        public IEnumerable<StatisticsDto> GetStatistics()
        {
            RowSet results = _session.Execute("SELECT * FROM statistics");
            return results.GetRows().Select(MapRowToStatisticsDto).ToList();
        }

        /// <summary>
        /// Increments a statistics counter.
        /// </summary>
        public void IncrementCounter(string counterName)
        {
            PreparedStatement prepared = _session.Prepare("UPDATE statistics SET counter_value = counter_value + 1 WHERE counter_name = ?");
            BoundStatement bound = prepared.Bind(counterName);
            _session.Execute(bound);
        }

        /// <summary>
        /// Decrements a statistics counter.
        /// </summary>
        public void DecrementCounter(string counterName)
        {
            PreparedStatement prepared = _session.Prepare("UPDATE statistics SET counter_value = counter_value - 1 WHERE counter_name = ?");
            BoundStatement bound = prepared.Bind(counterName);
            _session.Execute(bound);
        }

        /// <summary>
        /// Maps a Cassandra row to a StatisticsDto.
        /// </summary>
        private static StatisticsDto MapRowToStatisticsDto(Row row)
        {
            if (row == null) return null;

            return new StatisticsDto
            {
                CounterName = row.GetValue<string>("counter_name"),
                CounterValue = row.GetValue<long>("counter_value")
            };
        }
    }
}
