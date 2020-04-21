using System;
using System.Collections.Generic;

namespace AxisCapacity.Common
{
    public class Shift
    {
        private const string Unknown = "unknown";

        public enum ShiftEnum
        {
            Am,
            Pm
        }

        private readonly ShiftEnum _shift;

        public Shift(ShiftEnum shift)
        {
            _shift = shift;
        }

        public string Value()
        {
            switch (_shift)
            {
                case ShiftEnum.Am:
                    return "AM";
                case ShiftEnum.Pm:
                    return "PM";
                default:
                    return Unknown;
            }
        }

        public override string ToString()
        {
            return Value();
        }

        static Shift()
        {
            CreateStringMapping();
        }

        public static Shift From(string shiftName)
        {
            StringToShift.TryGetValue(shiftName.ToLower(), out var outValue);
            if (outValue == null)
            {
                throw new ArgumentException($"Shift '{shiftName}' not in list of shifts: {Utils.ToCsv(Values())}");
            }

            return outValue;
        }

        private static void CreateStringMapping()
        {
            foreach (var shift in Values())
            {
                StringToShift.Add(shift.Value().ToLower(), shift);
            }
        }

        public static IEnumerable<Shift> Values()
        {
            var shifts = (ShiftEnum[]) Enum.GetValues(typeof(ShiftEnum));
            foreach (var shift in shifts)
            {
                yield return new Shift(shift);
            }
        }

        private static readonly Dictionary<string, Shift> StringToShift = new Dictionary<string, Shift>();
    }
}