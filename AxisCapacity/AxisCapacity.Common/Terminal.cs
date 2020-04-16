using System;
using System.Collections.Generic;

namespace AxisCapacity.Common
{
    public class Terminal
    {
        private const string Unknown = "unknown";

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


        private readonly TerminalEnum _terminal;

        private Terminal(TerminalEnum terminal)
        {
            _terminal = terminal;
        }

        public string Value()
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
                    return Unknown;
            }
        }

        public override string ToString()
        {
            return Value();
        }

        static Terminal()
        {
            CreateStringMapping();
        }

        public static Terminal From(string terminalName)
        {
            StringToTerminal.TryGetValue(terminalName.ToLower(), out var outValue);
            if (outValue == null)
            {
                throw new ArgumentException($"Terminal '{terminalName}' not in list of terminals: {Utils.ToCsv(Values())}");
            }

            return outValue;
        }

        private static void CreateStringMapping()
        {
            foreach (var terminal in Values())
            {
                StringToTerminal.Add(terminal.Value().ToLower(), terminal);
            }
        }

        public static IEnumerable<Terminal> Values()
        {
            var values = (TerminalEnum[]) Enum.GetValues(typeof(TerminalEnum));
            foreach (var terminal in values)
            {
                yield return new Terminal(terminal);
            }
        }

        private static readonly Dictionary<string, Terminal> StringToTerminal = new Dictionary<string, Terminal>();
    }
}