namespace AxisCapacity.Engine
{
    public interface ICapacityEngine
    {
        decimal? CalculateCapacity(int? load, decimal? deliveries, int? shifts);
    }
}