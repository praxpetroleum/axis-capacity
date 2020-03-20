using System;
using System.Collections.Generic;
using AxisCapacity.Common;
using Microsoft.Data.SqlClient;

namespace AxisCapacity.Data
{
    public class CapacityRepository : ICapacityRepository
    {
        private const string CapacityTable = "dbo.Capacity";
        
        private readonly string _dbConnectionString;

        public CapacityRepository(string dbConnectionString)
        {
            _dbConnectionString = dbConnectionString;
        }

        public IEnumerable<CapacityResult> GetCapacities(Terminal terminal, ViewType view)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CapacityResult> GetCapacities(Terminal terminal, Shift shift, ViewType view)
        {
            throw new NotImplementedException();
        }

        public CapacityResult GetCapacity(Terminal terminal, Shift shift, DateTime date)
        {
            var day = date.ToString("ddd");

            using var connection = new SqlConnection(_dbConnectionString);
            using var cmd = new SqlCommand($"select avg_load, avg_deliveries, {day} from {CapacityTable} where terminal = @terminal and shift = @shift", connection);
            cmd.Parameters.AddWithValue("@terminal", terminal.StringValue());
            cmd.Parameters.AddWithValue("@shift", shift.StringValue());
            connection.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                return new CapacityResult((int) reader[0], (decimal) reader[1], (int) reader[2]);
            }

            return null;
        }

        public CapacityResult GetCapacity(Terminal terminal, Shift shift)
        {
            throw new NotImplementedException();
        }
    }
}