using System;
using System.Collections.Generic;

namespace AxisCapacity.Common
{
    public class Shift
    {
        private readonly ShiftEnum _shift;

        public Shift(ShiftEnum shift)
        {
            _shift = shift;
        }

        public string StringValue()
        {
            switch (_shift)
            {
                case ShiftEnum.Am:
                    return "AM";
                case ShiftEnum.Pm:
                    return "PM";
                default:
                    throw new ArgumentException("Invalid shift");
            }
        }


        static Shift ()
        {
            CreateStringMapping();
        }
        
        public static Shift From(string shiftName)
        {
            StringToTerminal.TryGetValue(shiftName.ToLower(), out var outValue);
            if (outValue == null)
            {
                throw new ArgumentException($"Invalid terminal '{shiftName}'");
            }
            return outValue;
        }

        private static void CreateStringMapping()
        {
            foreach (var shift in Values())
            {
                StringToTerminal.Add(shift.StringValue().ToLower(), shift);
            }
        }
        private static IEnumerable<Shift> Values()
        {
            var shifts = (ShiftEnum[]) Enum.GetValues(typeof(ShiftEnum));
            foreach (var shift in shifts)
            {
                yield return new Shift(shift);
            }
        }

        private static readonly Dictionary<string, Shift> StringToTerminal = new Dictionary<string, Shift>();
    }
    
    public enum ShiftEnum
    {
        Am, 
        Pm
    }
}