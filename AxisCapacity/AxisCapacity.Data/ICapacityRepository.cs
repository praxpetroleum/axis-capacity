using System;
using System.Collections.Generic;
using AxisCapacity.Common;

namespace AxisCapacity.Data
{
    public interface ICapacityRepository
    {
        IEnumerable<CapacityResult> GetCapacities(Terminal terminal, ViewType view);
        
        IEnumerable<CapacityResult> GetCapacities(Terminal terminal, Shift shift, ViewType view);
        
        CapacityResult GetCapacity(Terminal terminal, Shift shift, DateTime date);

        CapacityResult GetCapacity(Terminal terminal, Shift shift);
    }
}