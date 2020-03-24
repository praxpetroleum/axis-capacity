using System;

namespace AxisCapacity.Web.Model
{
    public class CapacityRequest
    {
        // Needed for de-seraliasation
        public CapacityRequest()
        {
        }

        public string OrderId {get; set;}

        public decimal? Amount {get; set;}

        public string Terminal {get; set;}
        
        public string Shift {get; set;}

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(OrderId) &&
                   Amount != null && 
                   Common.Terminal.From(Terminal) != null &&
                   Common.Shift.From(Shift) != null;
        }
    }
}