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
        public int AverageLoad { get; set; }

        [Name("dels_shift")]
        public decimal DeliveriesPerShift { get; set; }

        [Name("shifts")]
        public int NumberOfShifts { get; set; }

        [Name("capacity")]
        public decimal Capacity { get; set; }

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