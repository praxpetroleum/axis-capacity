using System;
using System.Collections.Generic;

namespace AxisCapacity.Data
{
    public interface ICapacityRepository
    {
        DbCapacity GetCapacity(string terminal, string shift, DateTime date);
        
        IEnumerable<DbCapacity> GetCapacities(string terminal, string shift, DateTime date, int groupId);
        
        IEnumerable<DbCapacity> GetCapacities(string terminal, string shift, DateTime? date);

        void InsertCapacity(DbCapacity dbCapacity);
    }
}