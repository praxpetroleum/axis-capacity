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
        
        private const string CapacityDateTable = "dbo.DateCapacities";

        private const string GroupingTable = "dbo.TerminalGroups";

        private readonly string _dbConnectionString;

        public CapacityRepository(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public DbCapacity GetCapacity(string terminal, string shift, DateTime date)
        {
            var sql = "select coalesce(c.terminal, dc.terminal)                     as terminal, " +
                      "c.day                                                as day, " +
                      "coalesce(c.shift, dc.shift)                          as shift, " +
                      "coalesce(dc.load, c.Load)                            as load, " +
                      "coalesce(dc.deliveries, c.deliveries)                as deliveries, " +
                      "coalesce(dc.shifts, c.shifts)                        as shifts, " +
                      "coalesce(dc.capacity, c.capacity)                    as capacity, " +
                      "dc.date                                              as date, " +
                      "g.group_id                                           as group_id " +
                      $"from {CapacityTable} c " +
                      $"full outer join {CapacityDateTable} dc " +
                      "on c.Terminal = dc.Terminal and c.Shift = dc.Shift and c.Day = substring(datename(dw, dc.date), 1, 3) " +
                      $"left outer join {GroupingTable} g on coalesce(c.terminal, dc.terminal) = g.terminal " +
                      "where (dc.Date = @date or c.Day = @day) " +
                      "and coalesce(c.Terminal, dc.Terminal) = @terminal " +
                      "and coalesce(c.shift, dc.shift) = @shift";

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@terminal", terminal);
            cmd.Parameters.AddWithValue("@day", date.ToString("ddd"));
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@shift", shift);

            connection.Open();

            using var reader = cmd.ExecuteReader();

            return reader.Read()
                ? new DbCapacity
                {
                    Terminal = (string) reader[0],
                    Day = reader[1] == DBNull.Value ? null : (string) reader[1],
                    Shift = (string) reader[2],
                    Load = reader[3] == DBNull.Value ? null : (int?) reader[3],
                    Deliveries = reader[4] == DBNull.Value ? null : (decimal?) reader[4],
                    Shifts = reader[5] == DBNull.Value ? null : (byte?) reader[5],
                    Capacity = reader[6] == DBNull.Value ? null : (decimal?) reader[6], 
                    Date = reader[7] == DBNull.Value ? null : (DateTime?) reader[7],
                    GroupId = reader[8] == DBNull.Value ? null : (byte?) reader[8]
                }
                : null;
        }


        public IEnumerable<DbCapacity> GetGroupCapacities(string terminal, string shift, DateTime date, int groupId)
        {
            var sql = "select coalesce(c.terminal, dc.terminal)                     as terminal, " +
                      "c.day                                                as day, " +
                      "coalesce(c.shift, dc.shift)                          as shift, " +
                      "coalesce(dc.load, c.Load)                            as load, " +
                      "coalesce(dc.deliveries, c.deliveries)                as deliveries, " +
                      "coalesce(dc.shifts, c.shifts)                        as shifts, " +
                      "coalesce(dc.capacity, c.capacity)                    as capacity, " +
                      "dc.date                                              as date, " +
                      "g.group_id                                           as group_id " +
                      $"from {CapacityTable} c " +
                      $"full outer join {CapacityDateTable} dc " +
                      "on c.Terminal = dc.Terminal and c.Shift = dc.Shift and c.Day = substring(datename(dw, date), 1, 3) " +
                      $"left outer join {GroupingTable} g on coalesce(c.terminal, dc.terminal) = g.terminal " +
                      "where (dc.Date = @date or c.Day = @day) " +
                      "and coalesce(c.Terminal, dc.Terminal) != @terminal " +
                      "and coalesce(c.shift, dc.shift) = @shift";

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@terminal", terminal);
            cmd.Parameters.AddWithValue("@day", date.ToString("ddd"));
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy/MM/dd"));
            cmd.Parameters.AddWithValue("@shift", shift);

            connection.Open();

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                yield return new DbCapacity
                {
                    Terminal = (string) reader[0],
                    Day = reader[1] == DBNull.Value ? null : (string) reader[1],
                    Shift = (string) reader[2],
                    Load = reader[3] == DBNull.Value ? null : (int?) reader[3],
                    Deliveries = reader[4] == DBNull.Value ? null : (decimal?) reader[4],
                    Shifts = reader[5] == DBNull.Value ? null : (byte?) reader[5],
                    Capacity = reader[6] == DBNull.Value ? null : (decimal?) reader[6],
                    Date = reader[7] == DBNull.Value ? null : (DateTime?) reader[7],
                    GroupId = reader[8] == DBNull.Value ? null : (byte?) reader[8]
                };
            }
        }

        public IEnumerable<DbCapacity> GetCapacities(string terminal, string shift, DateTime? date)
        {
            return date.HasValue ? GetCapacitiesWithDate(terminal, shift, date.Value) : GetCapacitiesWithNoDate(terminal, shift);
        }

        private IEnumerable<DbCapacity> GetCapacitiesWithDate(string terminal, string shift, DateTime date)
        {
            var sql = "select coalesce(c.Terminal, dc.Terminal)                     as terminal, " +  
                              "coalesce(c.Day, substring(datename(dw, date), 1, 3)) as day, " +
                              "coalesce(c.shift, dc.Shift)                          as shift, " + 
                              "coalesce(dc.load, c.Load)                            as load, " + 
                              "coalesce(dc.deliveries, c.deliveries)                as deliveries, " + 
                              "coalesce(dc.shifts, c.shifts)                        as shifts, " + 
                              "coalesce(dc.capacity, c.capacity)                    as capacity " + 
                              $"from {CapacityTable} c " + 
                              $"full outer join {CapacityDateTable} dc " +
                              "on c.Terminal = dc.Terminal and c.Shift = dc.Shift and c.Day = substring(datename(dw, date), 1, 3)" +
                              $"where (dc.Date = '{date:yyyy/MM/dd}' or c.Day='{date:ddd}')"; 



            if (!string.IsNullOrEmpty(terminal))
            {
                sql += $" and coalesce(c.terminal, dc.terminal) = '{terminal}'";
            }

            if (!string.IsNullOrEmpty(shift))
            {
                sql += $" and coalesce(c.shift, dc.shift) = '{shift}'";
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

        private IEnumerable<DbCapacity> GetCapacitiesWithNoDate(string terminal, string shift)
        {
            var sql = $"select terminal, day, shift, load, deliveries, shifts, capacity, date from (select terminal, shift, day, null as date, load, deliveries, shifts, capacity from {CapacityTable} union select terminal, shift, substring(datename(dw,date), 1, 3) day, date, load, deliveries, shifts, capacity from {CapacityDateTable}) as capacity_union where 1=1";


            if (!string.IsNullOrEmpty(terminal))
            {
                sql += $" and lower(terminal) = '{terminal}'";
            }

            if (!string.IsNullOrEmpty(shift))
            {
                sql += $" and lower(shift) = '{shift}'";
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
                    Date = reader[7] == DBNull.Value ? null : (DateTime?) reader[7], 
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

        public void InsertDateCapacity(DbCapacity dbCapacity)
        {
            var keyFields = new List<string> {"terminal", "date", "shift"};
            var nonKeyFields = new List<string> { "load", "deliveries", "shifts", "capacity" };
            IgnoreIfNull(nonKeyFields, dbCapacity);
            keyFields.AddRange(nonKeyFields);

            var sql = $"insert into {CapacityDateTable} ({ToCsv(keyFields)}) values ({ToCsv(keyFields, "@")})";

            using var connection = new SqlConnection(_dbConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@terminal", dbCapacity.Terminal);
            command.Parameters.AddWithValue("@shift", dbCapacity.Shift);
            command.Parameters.AddWithValue("@date", dbCapacity.Date);
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