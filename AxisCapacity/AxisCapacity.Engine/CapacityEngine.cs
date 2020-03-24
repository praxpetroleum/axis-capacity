namespace AxisCapacity.Engine
{
    public class CapacityEngine : ICapacityEngine
    {
        public decimal CalculateCapacity(Parameters parameters)
        {
            return parameters.AverageLoad * parameters.DeliveryPerShift * parameters.NumberOfShifts;
        }
    }
}