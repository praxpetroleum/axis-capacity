using System;
using System.Collections.Generic;
using AxisCapacity.Common;

namespace AxisCapacity.Data
{
    public interface ICapacityRepository
    {
        DbCapacity GetCapacity(Terminal terminal, Shift shift, DateTime date);
        
        IEnumerable<DbCapacity> GetGroupCapacities(Terminal terminal, Shift shift, DateTime date, int groupId);
        
        IEnumerable<DbCapacity> GetCapacities(Terminal terminal, Shift shift, DateTime? date);

        void InsertCapacity(DbCapacity dbCapacity);

        void InsertDateCapacity(DbCapacity dbCapacity);
    }
}