using System;
using AxisCapacity.Common;

namespace AxisCapacity.Data
{
    public interface ICapacityRepository
    {
        DbCapacity GetCapacity(string terminal, Shift shift, DateTime date);
        
        DbCapacity GetDateCapacity(string terminal, Shift shift, DateTime date);
        
        void InsertCapacity(DbCapacity dbCapacity);

        void InsertDateCapacity(DbCapacity dbCapacity);
    }
}