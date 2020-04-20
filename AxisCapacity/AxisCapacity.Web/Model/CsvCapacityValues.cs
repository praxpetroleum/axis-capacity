using System;
using CsvHelper.Configuration.Attributes;

namespace AxisCapacity.Web.Model
{
    public class CsvCapacityValues
    {
        [Name("depot")]
        public string Depot { get; set; }

        [Name("shift")]
        public string Shift { get; set; }

        [Name("day")]
        [OptionalAttribute]
        public string Day { get; set; }

        [Name("date")]
        [OptionalAttribute]
        public DateTime? Date { get; set; }

        [Name("avg_load")]
        [OptionalAttribute]
        public int? AverageLoad { get; set; }

        [Name("dels_shift")]
        [OptionalAttribute]
        public decimal? DeliveriesPerShift { get; set; }

        [Name("shifts")]
        [OptionalAttribute]
        public int? NumberOfShifts { get; set; }

        [Name("capacity")]
        [OptionalAttribute]
        public decimal? Capacity { get; set; }

        public override string ToString()
        {
            return "CsvCapacityValues{" + 
                   "Depot='" + Depot + "'" + 
                   " Shift='" + Shift + "'" + 
                   " Day='" + Day + "'" + 
                   " Date='" + Date?.ToString("yyyy/MM/dd") + "'" + 
                   " AvgLoad=" + AverageLoad + 
                   " DelsShift=" + DeliveriesPerShift + 
                   " Shifts=" + NumberOfShifts + 
                   " Capacity=" + Capacity +
                   "}";
        }
    }
}