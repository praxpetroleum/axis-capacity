using System;
using System.Collections.Generic;
using AxisCapacity.Common;

namespace AxisCapacity.Data
{
    public interface ICapacityRepository
    {
        DbCapacity GetCapacity(string terminal, Shift shift, DateTime date);
        
        DbCapacity GetDateCapacity(string terminal, Shift shift, DateTime date);

        IEnumerable<DbCapacity> GetCapacities(string terminal, Shift shift, DateTime? date);

        IEnumerable<DbCapacity> GetDateCapacities(string terminal, Shift shift, DateTime? start, DateTime? end);
        
        void InsertCapacity(DbCapacity dbCapacity);

        void InsertDateCapacity(DbCapacity dbCapacity);
    }
}