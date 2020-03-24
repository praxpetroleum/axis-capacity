namespace AxisCapacity.Engine
{
    public class ParameterBuilder
    {
        private int _averageLoad;
        private decimal _deliveriesPerShift;
        private int _shifts;


        public ParameterBuilder WithAverageLoad(int averageLoad)
        {
            _averageLoad = averageLoad;
            return this;
        }

        public ParameterBuilder WithDeliveriesPerShift(decimal deliveriesPerShift)
        {
            _deliveriesPerShift = deliveriesPerShift;
            return this;
        }

        public ParameterBuilder WithShifts(int shifts)
        {
            _shifts = shifts;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters(_averageLoad, _deliveriesPerShift, _shifts);
        }
    }

    public class Parameters
    {
        internal int AverageLoad { get; private set; }
        internal decimal DeliveryPerShift { get; private set; }
        internal int NumberOfShifts { get; private set; }

        internal Parameters(int averageLoad, decimal deliveryPerShift, int numberOfShifts)
        {
            AverageLoad = averageLoad;
            DeliveryPerShift = deliveryPerShift;
            NumberOfShifts = numberOfShifts;
        }
    }
}