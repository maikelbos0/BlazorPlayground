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

        if (symbol is UnaryOperator unaryOpeator) {
            if (unaryOpeator.Symbol == null) {
                symbols.RemoveAt(symbols.Count - 1);
            }
            else {
                symbols[symbols.Count - 1] = unaryOpeator.Symbol;
            }
            return true;
        }

        if (symbol is ComposableNumber number) {
            if (number.Characters.Count <= 1) {
                symbols.RemoveAt(symbols.Count - 1);
                return true;
            }
            return number.TryRemoveCharacter();
        }

        // TODO add backspace for groups

        return false;
    }
}
