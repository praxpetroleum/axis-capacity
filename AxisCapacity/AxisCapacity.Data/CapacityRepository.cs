using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxisCapacity.Common;
using Microsoft.Data.SqlClient;

namespace AxisCapacity.Data
{
    public class CapacityRepository : ICapacityRepository
    {
        private const string CapacityTable = "dbo.DepotCapacities";
        private const string CapacityDateTable = "dbo.DepotDateCapacities";
        private const string DepotTable = "dbo.Depot";  
        private const string TerminalTable = "dbo.Terminals";

        private readonly string _dbConnectionString;

        public CapacityRepository(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public DbCapacity GetCapacity(string terminal, Shift shift, DateTime date)
        {
            var sql = "select " +
                      "d.name                              as  depot, " +
                      "dc.day                              as  day,  " +
                      "dc.shift                            as  shift, " +
                      "dc.load                             as  load, " +
                      "dc.deliveries                       as  deliveries, " +
                      "dc.shifts                           as  shifts, " +
                      "dc.capacity                         as  capacity " + 
                      $"from {TerminalTable} t inner join {DepotTable} d on t.depot_id=d.id inner join {CapacityTable} dc on dc.depot_id=d.id " + 
                      "where t.name = @terminal and dc.shift = @shift and dc.day = @day";

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@terminal", terminal);
            cmd.Parameters.AddWithValue("@day", date.ToString("ddd"));
            cmd.Parameters.AddWithValue("@shift", shift.Value());

            connection.Open();

            using var reader = cmd.ExecuteReader();

            return reader.Read()
                ? new DbCapacity
                {
                    Depot = (string) reader[0],
                    Day = reader[1] == DBNull.Value ? null : (string) reader[1],
                    Shift = (string) reader[2],
                    Load = reader[3] == DBNull.Value ? null : (int?) reader[3],
                    Deliveries = reader[4] == DBNull.Value ? null : (decimal?) reader[4],
                    Shifts = reader[5] == DBNull.Value ? null : (int?) reader[5],
                    Capacity = reader[6] == DBNull.Value ? null : (decimal?) reader[6], 
                }
                : null;
        }

        public DbCapacity GetDateCapacity(string terminal, Shift shift, DateTime date)
        {
            var sql = "select " +
                      "d.name                              as  depot, " +
                      "dc.date                             as  date,  " +
                      "dc.shift                            as  shift, " +
                      "dc.load                             as  load, " +
                      "dc.deliveries                       as  deliveries, " +
                      "dc.shifts                           as  shifts, " +
                      "dc.capacity                         as  capacity " + 
                      $"from {TerminalTable} t inner join {DepotTable} d on t.depot_id=d.id inner join {CapacityDateTable} dc on dc.depot_id=d.id " + 
                      "where t.name = @terminal and dc.shift = @shift and dc.date = @date";

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@terminal", terminal);
            cmd.Parameters.AddWithValue("@shift", shift.Value());
            cmd.Parameters.AddWithValue("@date", date.ToString("yyyy/MM/dd"));

            connection.Open();

            using var reader = cmd.ExecuteReader();

            return reader.Read()
                ? new DbCapacity
                {
                    Depot = (string) reader[0],
                    Date = reader[1] == DBNull.Value ? null : (DateTime?) reader[1],
                    Shift = (string) reader[2],
                    Load = reader[3] == DBNull.Value ? null : (int?) reader[3],
                    Deliveries = reader[4] == DBNull.Value ? null : (decimal?) reader[4],
                    Shifts = reader[5] == DBNull.Value ? null : (int?) reader[5],
                    Capacity = reader[6] == DBNull.Value ? null : (decimal?) reader[6], 
                }
                : null;
        }

        public void InsertCapacity(DbCapacity dbCapacity)
        {
            var special = new[] {$"(select id from {DepotTable} where name = @depot)", "depot_id"};
            var keyFieldsMinusSpecial = new List<string> {"day", "shift"};
            var nonKeyFields = new List<string> { "load", "deliveries", "shifts", "capacity" };
            var allFields = keyFieldsMinusSpecial;
            IgnoreIfNull(nonKeyFields, dbCapacity);
            allFields.AddRange(nonKeyFields);

            var sql = $"merge into {CapacityTable} as target " + 
                      $"using (select {special[0]},{Utils.ToCsv(allFields, "@")}) as source({special[1]},{Utils.ToCsv(allFields)}) " + 
                      "on target.depot_id = source.depot_id and target.day = source.day and target.shift = source.shift " + 
                      (nonKeyFields.Any() ? $"when matched then update set {CreateUpdates(nonKeyFields, "target", "source")} " : string.Empty) + 
                      $"when not matched then insert({special[1]},{Utils.ToCsv(allFields)}) values ({special[0]},{Utils.ToCsv(allFields, "source.")});";

            using var connection = new SqlConnection(_dbConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@depot", dbCapacity.Depot);
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
            var special = new[] {$"(select id from {DepotTable} where name = @depot)", "depot_id"};
            var keyFieldsMinusSpecial = new List<string> {"date", "shift"};
            var nonKeyFields = new List<string> { "load", "deliveries", "shifts", "capacity" };
            var allFields = keyFieldsMinusSpecial;
            IgnoreIfNull(nonKeyFields, dbCapacity);
            allFields.AddRange(nonKeyFields);

            var sql = $"insert into {CapacityDateTable} ({special[1]},{Utils.ToCsv(allFields)}) values ({special[0]},{Utils.ToCsv(allFields, "@")})";

            using var connection = new SqlConnection(_dbConnectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@depot", dbCapacity.Depot);
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
    }
}