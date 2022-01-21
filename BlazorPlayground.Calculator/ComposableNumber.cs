using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class ComposableNumber : EvaluatableSymbol {
        private static readonly HashSet<char> decimalSeparators = new() { '.', ',' };

        internal List<char> Characters { get; } = new();

        public bool Append(char c) {
            if (!char.IsDigit(c) && !decimalSeparators.Contains(c)) {
                return false;
            }

            if (!char.IsDigit(c) && decimalSeparators.Any(s => Characters.Contains(s))) {
                return false;
            }

            Characters.Add(c);
            return true;
        }

        internal override decimal Evaluate() {
            var value = "0";

            if (Characters.Any()) {
                value = new string(Characters.Select(Normalize).ToArray());
            }

            return decimal.Parse(value, CultureInfo.InvariantCulture);
        }

        private char Normalize(char c) {
            if (decimalSeparators.Contains(c)) {
                return '.';
            }

            return c;
        }

        override public string ToString() {
            if (Characters.Any()) {
                return new string(Characters.ToArray());
            }

            return $"0";
        }
    }
}
