using System;
using System.Collections.Generic;
using AxisCapacity.Common;
using Microsoft.Data.SqlClient;

namespace AxisCapacity.Data
{
    public class CapacityRepository : ICapacityRepository
    {
        private const string CapacityTable = "dbo.Capacity";

        private const string GroupingTable = "dbo.TerminalGrouping";
        
        private readonly string _dbConnectionString;

        public CapacityRepository(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public CapacityResult GetCapacity(Terminal terminal, Shift shift)
        {
            return GetCapacity(terminal, shift, DateTime.Today);
        }

        public IEnumerable<CapacityResult> GetGroupCapacities(Terminal terminal, Shift shift, int? groupId)
        {
            return GetGroupCapacities(terminal, shift, DateTime.Today, groupId);
        }

        public CapacityResult GetCapacity(Terminal terminal, Shift shift, DateTime date)
        {
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date));
            }
            
            var day = date.ToString("ddd");

            var sql = $"select c.Terminal, c.avg_load, c.avg_deliveries, c.{day}, g.group_id from {CapacityTable} c LEFT OUTER JOIN {GroupingTable} g on c.Terminal = g.Terminal where c.terminal = @terminal and c.shift = @shift";

            using var connection = new SqlConnection(_dbConnectionString);
            
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@terminal", terminal.StringValue());
            cmd.Parameters.AddWithValue("@shift", shift.StringValue());
            
            connection.Open();
            
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                return new CapacityResult((string) reader[0], (int) reader[1], (decimal) reader[2], (byte) reader[3])
                {
                    GroupId = reader[4] == DBNull.Value ? null : (byte?) reader[4]
                };
            }

            return null;
        }

        public IEnumerable<CapacityResult> GetGroupCapacities(Terminal terminal, Shift shift, DateTime date, int? groupId)
        {
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date));
            }

            if (groupId == null)
            {
                throw new ArgumentNullException(nameof(groupId));
            }
            
            var day = date.ToString("ddd");

            var sql = $"select c.Terminal, c.avg_load, c.avg_deliveries, c.{day}, g.group_id from {CapacityTable} c INNER JOIN {GroupingTable} g on c.Terminal = g.Terminal where c.terminal != @terminal and c.shift = @shift";

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@terminal", terminal.StringValue());
            cmd.Parameters.AddWithValue("@shift", shift.StringValue());
            connection.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new CapacityResult((string) reader[0], (int) reader[1], (decimal) reader[2], (byte) reader[3])
                {
                    GroupId = (byte?) reader[4]
                };
            }
        }
        
    }
}