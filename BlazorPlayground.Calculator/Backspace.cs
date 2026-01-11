using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator;

internal class Backspace : ISymbol {
    public bool TryAppendTo(IList<ISymbol> symbols) {
        var symbol = symbols.LastOrDefault();

        if (symbol is BinaryOperator) {
            symbols.RemoveAt(symbols.Count - 1);
            return true;
        }

        while (symbol is UnaryOperator op) {
            symbol = op.Symbol;
        }

        if (symbol is ComposableNumber number) {
            return number.TryRemoveCharacter();
            // TODO remove if no more characters
        }

        // TODO add backspace for unary operators
        // TODO add backspace for groups

        return false;
    }
}
