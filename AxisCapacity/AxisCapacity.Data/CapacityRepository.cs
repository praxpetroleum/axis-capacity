using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var keyFields = new List<string> {"terminal", "day", "shift"};
            var nonKeyFields = new List<string> { "load", "deliveries", "shifts", "capacity" };
            IgnoreIfNull(nonKeyFields, dbCapacity);
            keyFields.AddRange(nonKeyFields);


            var sql = $"merge into {CapacityTable} as target " + 
                      $"using (select {ToCsv(keyFields, "@")}) as source({ToCsv(keyFields)}) " + 
                      "on target.terminal = source.terminal and target.day = source.day and target.shift = source.shift " + 
                      (nonKeyFields.Any() ? $"when matched then update set {CreateUpdates(nonKeyFields, "target", "source")} " : string.Empty) + 
                      $"when not matched then insert({ToCsv(keyFields)}) values ({ToCsv(keyFields, "source.")});";

            using var connection = new SqlConnection(_dbConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@terminal", dbCapacity.Terminal);
            command.Parameters.AddWithValue("@day", dbCapacity.Day);
            command.Parameters.AddWithValue("@shift", dbCapacity.Shift);
            if (dbCapacity.Load.HasValue)
            {
                command.Parameters.AddWithValue("@load", dbCapacity.Load.Value);
            }
            if (dbCapacity.Deliveries.HasValue)
            {
                command.Parameters.AddWithValue("@deliveries", dbCapacity.Deliveries.Value);
            }
            if (dbCapacity.Shifts.HasValue)
            {
                command.Parameters.AddWithValue("@shifts",  dbCapacity.Shifts.Value);
            }

            if (dbCapacity.Capacity.HasValue)
            {
                command.Parameters.AddWithValue("@capacity",  dbCapacity.Capacity.Value);
            }
            connection.Open();
            command.ExecuteNonQuery();
        }

        private static void IgnoreIfNull(IList nonKeyFields, DbCapacity dbCapacity)
        {
            RemoveIfNull(nonKeyFields, dbCapacity.Load, "load");
            RemoveIfNull(nonKeyFields, dbCapacity.Deliveries, "deliveries");
            RemoveIfNull(nonKeyFields, dbCapacity.Shifts, "shifts");
            RemoveIfNull(nonKeyFields, dbCapacity.Capacity, "capacity");
        }

        private static void RemoveIfNull<T>(IList fields, T? field, string fieldName) where T : struct
        {
            if (!field.HasValue)
            {
                fields.Remove(fieldName);
            }
        }

        private static string CreateUpdates(IEnumerable<string> enumerable, string target, string source)
        {
            var sb = new StringBuilder();
            foreach (var item in enumerable)
            {
                sb.Append(target).Append(".").Append(item).Append("=").Append(source).Append(".").Append(item).Append(",");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
        
        private static string ToCsv(IEnumerable<string> enumerable)
        {
            return ToCsv(enumerable, string.Empty);
        }
        
        private static string ToCsv(IEnumerable<string> enumerable, string prefix)
        {
            var sb = new StringBuilder();
            foreach (var item in enumerable)
            {
                sb.Append(prefix).Append(item).Append(",");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }


    }
}