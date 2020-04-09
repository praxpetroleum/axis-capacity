using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace AxisCapacity.Data
{
    public class CapacityRepository : ICapacityRepository
    {
        private const string CapacityTable = "dbo.Capacities";

        private const string GroupingTable = "dbo.TerminalGroups";

        private readonly string _dbConnectionString;

        public CapacityRepository(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public DbCapacity GetCapacity(string terminal, string shift, DateTime date)
        {
            var day = date.ToString("ddd");
            var sql = $"select c.terminal, c.load, c.deliveries, c.shifts, c.capacity, g.group_id from {CapacityTable} c left outer join {GroupingTable} g on c.terminal = g.terminal where c.terminal = @terminal and c.day = @day and c.shift = @shift";

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@terminal", terminal);
            cmd.Parameters.AddWithValue("@day", day);
            cmd.Parameters.AddWithValue("@shift", shift);

            connection.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var capacity = new DbCapacity
                {
                    Terminal = (string) reader[0],
                    Load = reader[1] == DBNull.Value ? null : (int?) reader[1],
                    Deliveries = reader[2] == DBNull.Value ? null : (decimal?) reader[2],
                    Shifts = reader[3] == DBNull.Value ? null : (byte?) reader[3],
                    Capacity = reader[4] == DBNull.Value ? null : (decimal?) reader[4],
                    GroupId = reader[5] == DBNull.Value ? null : (byte?) reader[5]
                };
                return capacity;
            }

            return null;
        }


        public IEnumerable<DbCapacity> GetCapacities(string terminal, string shift, DateTime date, int groupId)
        {
            var day = date.ToString("ddd");
            var sql = $"select c.terminal, c.load, c.deliveries, c.shifts, c.capacity, g.group_id from {CapacityTable} c inner join {GroupingTable} g on c.terminal = g.terminal where c.terminal != @terminal and c.day = @day and c.shift = @shift";

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@terminal", terminal);
            cmd.Parameters.AddWithValue("@day", day);
            cmd.Parameters.AddWithValue("@shift", shift);

            connection.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new DbCapacity
                {
                    Terminal = (string)reader[0], 
                    Load = reader[1] == DBNull.Value ? null : (int?) reader[1], 
                    Deliveries = reader[2] == DBNull.Value ? null : (decimal?) reader[2], 
                    Shifts = reader[3] == DBNull.Value ? null : (byte?) reader[3], 
                    Capacity = reader[4] == DBNull.Value ? null : (decimal?) reader[3], 
                    GroupId = reader[4] == DBNull.Value ? null : (byte?) reader[4]
                };
            }
        }

        public IEnumerable<DbCapacity> GetCapacities(string terminal, string shift, DateTime? date)
        {
            var sql = $"select terminal, day, shift, load, deliveries, shifts, capacity from {CapacityTable} where 1=1";

            if (!string.IsNullOrEmpty(terminal))
            {
                sql += $" and lower(terminal) = '{terminal}'";
            }

            if (!string.IsNullOrEmpty(shift))
            {
                sql += $" and lower(shift) = '{shift}'";
            }

            if (date.HasValue)
            {
                sql += $" and lower(day) = '{date.Value:ddd}'";
            }

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand(sql, connection);
            connection.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new DbCapacity
                {
                    Terminal = (string) reader[0],
                    Day = (string) reader[1],
                    Shift = (string) reader[2],
                    Load = reader[3] == DBNull.Value ? null : (int?) reader[3],
                    Deliveries = reader[4] == DBNull.Value ? null : (decimal?) reader[4],
                    Shifts = reader[5] == DBNull.Value ? null : (byte?) reader[5],
                    Capacity = reader[6] == DBNull.Value ? null : (decimal?) reader[6], 
                };
            }
        }

        public void InsertCapacity(DbCapacity dbCapacity)
        {
            var sql = $"merge into {CapacityTable} as target " + 
                      "using (select @terminal, @day, @shift, @load, @deliveries, @shifts, @capacity) as source(terminal, day, shift, load, deliveries, shifts, capacity) " + 
                      "on target.terminal = source.terminal and target.day = source.day and target.shift = source.shift " + 
                      "when matched then update set target.load = source.load, target.deliveries = source.deliveries, target.shifts = source.shifts, target.capacity = source.capacity " + 
                      "when not matched then insert(terminal, day, shift, load, deliveries, shifts, capacity) values (source.terminal, source.day, source.shift, source.load, source.deliveries, source.shifts, source.capacity);";

            using var connection = new SqlConnection(_dbConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@terminal", dbCapacity.Terminal);
            command.Parameters.AddWithValue("@day", dbCapacity.Day);
            command.Parameters.AddWithValue("@shift", dbCapacity.Shift);
            command.Parameters.AddWithValue("@load", dbCapacity.Load.HasValue ? (object) dbCapacity.Load.Value : DBNull.Value);
            command.Parameters.AddWithValue("@deliveries", dbCapacity.Deliveries.HasValue ? (object) dbCapacity.Deliveries.Value : DBNull.Value);
            command.Parameters.AddWithValue("@shifts", dbCapacity.Shifts.HasValue ? (object) dbCapacity.Shifts.Value : DBNull.Value);
            command.Parameters.AddWithValue("@capacity", dbCapacity.Capacity.HasValue ? (object) dbCapacity.Capacity.Value : DBNull.Value);
            connection.Open();
            command.ExecuteNonQuery();
        }

    }
}