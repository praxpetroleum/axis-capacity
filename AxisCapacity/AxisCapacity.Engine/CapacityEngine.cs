namespace AxisCapacity.Engine
{
    public class CapacityEngine : ICapacityEngine
    {
        public decimal? CalculateCapacity(int? load, decimal? deliveries, int? shifts)
        {
            if (load.HasValue && deliveries.HasValue && shifts.HasValue)
            {
                return load.Value * deliveries.Value * shifts.Value;
            }
            return null;
        }
    }
}