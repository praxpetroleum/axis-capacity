using System;
using System.Collections.Generic;

namespace AxisCapacity.Common
{
    public class View
    {
        private const string Unknown = "unknown";

        public enum ViewEnum
        {
            Month,
            Day,
            Week
        }

        private readonly ViewEnum _view;

        public View(ViewEnum view)
        {
            _view = view;
        }

        public string Value()
        {
            switch (_view)
            {
                case ViewEnum.Day:
                    return "Day";
                case ViewEnum.Month:
                    return "Month";
                case ViewEnum.Week:
                    return "Week";
                default:
                    return Unknown;
            }
        }

        static View()
        {
            CreateStringMapping();
        }

        public override string ToString()
        {
            return Value();
        }

        public bool IsMonth()
        {
            return _view == ViewEnum.Month;
        }

        public static View From(string viewName)
        {
            StringToView.TryGetValue(viewName.ToLower(), out var outValue);
            if (outValue == null)
            {
                throw new ArgumentException($"View '{viewName}' not in list of views: {Utils.ToCsv(Values())}");
            }

            return outValue;
        }

        public static IEnumerable<View> Values()
        {
            var views = (ViewEnum[]) Enum.GetValues(typeof(ViewEnum));
            foreach (var view in views)
            {
                yield return new View(view);
            }
        }

        private static void CreateStringMapping()
        {
            foreach (var value in Values())
            {
                StringToView.Add(value.Value().ToLower(), value);
            }
        }

        private static readonly Dictionary<string, View> StringToView = new Dictionary<string, View>();
    }
}