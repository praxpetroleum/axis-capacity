namespace AxisCapacity.Data
{
    public class CapacityResult
    {
        public CapacityResult(int averageLoad, decimal deliveriesPerShift, int numberOfShifts)
        {
            AverageLoad = averageLoad;
            DeliveriesPerShift = deliveriesPerShift;
            NumberOfShifts = numberOfShifts;
        }

        public int AverageLoad { get; }

        public decimal DeliveriesPerShift { get; }

        public int NumberOfShifts { get; }

        public decimal Capacity { get; set; }
    }
}