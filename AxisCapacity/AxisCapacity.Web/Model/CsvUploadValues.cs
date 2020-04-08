namespace AxisCapacity.Web.Model
{
    public class CsvUploadValues
    {
        public string Terminal { get; set; }

        public string Shift { get; set; }

        public string Day { get; set; }

        public int AverageLoad { get; set; }

        public decimal DeliveriesPerShift { get; set; }

        public int NumberOfShifts { get; set; }

        public decimal Capacity { get; set; }
    }
}