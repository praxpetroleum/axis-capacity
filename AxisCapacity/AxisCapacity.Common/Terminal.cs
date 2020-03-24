using System;
using System.Collections.Generic;

namespace AxisCapacity.Common
{
    public class Terminal
    {
        private readonly TerminalEnum _terminal;

        private Terminal(TerminalEnum terminal)
        {
            _terminal = terminal;
        }

        public string StringValue()
        {
            switch (_terminal)
            {
                case TerminalEnum.Grangemouth:
                    return "Grangemouth";
                case TerminalEnum.Grays:
                    return "Grays";
                case TerminalEnum.Dagenham:
                    return "Dagenham";
                case TerminalEnum.Immingham:
                    return "Immingham";
                case TerminalEnum.Jarrow:
                    return "Jarrow";
                case TerminalEnum.Kingsbury:
                    return "Kingsbury";
                case TerminalEnum.Westerleigh:
                    return "Westerleigh";
                default:
                    throw new ArgumentException("Invalid terminal");
            }
        }

        

        static Terminal ()
        {
            CreateStringMapping();
        }
        
        public static Terminal From(string terminalName)
        {
            StringToTerminal.TryGetValue(terminalName.ToLower(), out var outValue);
            if (outValue == null)
            {
                throw new ArgumentException($"Invalid terminal '{terminalName}'");
            }
            return outValue;
        }

        private static void CreateStringMapping()
        {
            foreach (var terminal in Values())
            {
                StringToTerminal.Add(terminal.StringValue().ToLower(), terminal);
            }
        }
        private static IEnumerable<Terminal> Values()
        {
            var values = (TerminalEnum[]) Enum.GetValues(typeof(TerminalEnum));
            foreach (var terminal in values)
            {
                yield return new Terminal(terminal);
            }
        }

        private static readonly Dictionary<string, Terminal> StringToTerminal = new Dictionary<string, Terminal>();
    }


    public enum TerminalEnum
    {
        Dagenham,
        Grays,
        Westerleigh,
        Kingsbury,
        Immingham,
        Jarrow,
        Grangemouth
    }
}