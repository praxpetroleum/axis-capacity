using CsvHelper.Configuration.Attributes;

namespace AxisCapacity.Web.Model
{
    public class CsvCapacityValues
    {
        [Name("terminal")]
        public string Terminal { get; set; }

        [Name("shift")]
        public string Shift { get; set; }

        [Name("day")]
        public string Day { get; set; }

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
                   "Terminal='" + Terminal + "'" + 
                   " Shift='" + Shift + "'" + 
                   " Day='" + Day + "'" + 
                   " AvgLoad=" + AverageLoad + 
                   " DelsShift=" + DeliveriesPerShift + 
                   " Shifts=" + NumberOfShifts + 
                   " Capacity=" + Capacity +
                   "}";
        }
    }
}