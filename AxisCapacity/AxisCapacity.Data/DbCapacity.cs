using System;

namespace AxisCapacity.Data
{
    public class DbCapacity
    {
        public string Terminal { get; set; }
        public string Day { get; set; }
        public string Shift { get; set; }
        public int? Load { get; set; }
        public decimal? Deliveries { get; set; }
        public int? Shifts { get; set; }
        public decimal? Capacity { get; set; }
        public int? GroupId { get; set; }

        public override string ToString()
        {
            return "DbCapacity{" + 
                   "Terminal='" + Terminal + "'" + 
                   " Day='" + Day + "'" + 
                   " Shift='" + Shift + "'" + 
                   "}";
        }
    }
}