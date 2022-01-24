using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class ComposableNumber : EvaluatableSymbol {
        private static readonly HashSet<char> decimalSeparators = new() { '.', ',' };

        internal List<char> Characters { get; } = new();

        internal bool TryAppend(char character) {
            if (!char.IsDigit(character) && !decimalSeparators.Contains(character)) {
                return false;
            }

            if (!char.IsDigit(character) && decimalSeparators.Any(s => Characters.Contains(s))) {
                return false;
            }

            Characters.Add(character);
            return true;
        }

        internal bool TryRemoveCharacter() {
            if (Characters.Count == 0) {
                return false;
            }

            Characters.RemoveAt(Characters.Count - 1);
            return true;
        }

        internal override decimal Evaluate() {
            if (Characters.Any()) {
                return decimal.Parse(new string(Characters.Select(Normalize).ToArray()), CultureInfo.InvariantCulture);
            }

            return 0M;
        }

        private char Normalize(char character) {
            if (decimalSeparators.Contains(character)) {
                return '.';
            }

            return character;
        }

        override public string ToString() {
            if (Characters.Any()) {
                return new string(Characters.ToArray());
            }

            return $"0";
        }
    }
}
