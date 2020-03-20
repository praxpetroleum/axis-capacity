namespace AxisCapacity.Data
{
    public class CapacityResult
    {
        public CapacityResult(string name, int averageLoad, decimal deliveriesPerShift, int numberOfShifts)
        {
            Name = name;
            AverageLoad = averageLoad;
            DeliveriesPerShift = deliveriesPerShift;
            NumberOfShifts = numberOfShifts;
        }

        public string Name { get; }

        public int AverageLoad { get; }

        public decimal DeliveriesPerShift { get; }

        public int NumberOfShifts { get; }

        public decimal Capacity { get; set; }

        public int? GroupId { get; set; }
    }
}