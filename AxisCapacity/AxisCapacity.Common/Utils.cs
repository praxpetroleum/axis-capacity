using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AxisCapacity.Common
{
    public static class Utils
    {
        public static string ToCsv<T>(IEnumerable<T> enumerable) where T : class
        {
            return ToCsv(enumerable.Select(item => item.ToString()));
        }

        
        public static string ToCsv(IEnumerable<string> enumerable)
        {
            return ToCsv(enumerable, string.Empty);
        }

        public static string ToCsv(IEnumerable<string> enumerable, string prefix)
        {
            var sb = new StringBuilder();
            foreach (var item in enumerable)
            {
                sb.Append(prefix).Append(item).Append(",");
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }
    }
}