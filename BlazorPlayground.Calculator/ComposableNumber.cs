using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BlazorPlayground.Calculator;

internal class ComposableNumber : EvaluatableSymbol {
    private static readonly HashSet<char> decimalSeparators = ['.', ','];

    internal List<char> Characters { get; } = [];

    internal bool TryAppend(char character) {
        if (!char.IsDigit(character) && !decimalSeparators.Contains(character)) {
            return false;
        }

        if (!char.IsDigit(character) && decimalSeparators.Any(Characters.Contains)) {
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
        if (Characters.Count != 0) {
            return decimal.Parse(new string([.. Characters.Select(Normalize)]), CultureInfo.InvariantCulture);
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
        if (Characters.Count != 0) {
            return new string([.. Characters]);
        }

        return $"0";
    }
}
