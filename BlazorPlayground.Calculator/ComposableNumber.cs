using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class ComposableNumber : IEvaluatableSymbol {
        private static readonly HashSet<char> decimalSeparators = new() { '.', ',' };
        private static readonly HashSet<char> minusCharacters = new() { '-', '−', '±' };

        internal bool IsNegative { get; set; } = false;
        internal List<char> Characters { get; } = new();

        public bool Append(char c) {
            if (minusCharacters.Contains(c)) {
                IsNegative = !IsNegative;
                return true;
            }

            if (!char.IsDigit(c) && !decimalSeparators.Contains(c)) {
                return false;
            }

            if (!char.IsDigit(c) && decimalSeparators.Any(s => Characters.Contains(s))) {
                return false;
            }

            Characters.Add(c);
            return true;
        }

        public decimal Evaluate() {
            var value = new string(Characters.Select(Normalize).ToArray());

            if (IsNegative) {
                return -decimal.Parse(value, CultureInfo.InvariantCulture);
            }
            else {
                return decimal.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        private char Normalize(char c) {
            if (decimalSeparators.Contains(c)) {
                return '.';
            }

            return c;
        }
    }
}
