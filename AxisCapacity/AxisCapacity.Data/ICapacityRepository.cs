using System;
using System.Collections.Generic;
using AxisCapacity.Common;

namespace AxisCapacity.Data
{
    public interface ICapacityRepository
    {
        CapacityResult GetCapacity(Terminal terminal, Shift shift, DateTime date);

        IEnumerable<CapacityResult> GetGroupCapacities(Terminal terminal, Shift shift, DateTime date, int? groupId);
    }
}