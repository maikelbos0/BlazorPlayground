using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class SymbolGroup : IEvaluatableSymbol {
        internal List<ISymbol> Symbols { get; } = new();
        internal bool LastSymbolIsEvaluatable => Symbols.LastOrDefault() is IEvaluatableSymbol;

        internal bool Append(ISymbol symbol) {
            if (LastSymbolIsEvaluatable == symbol is not IEvaluatableSymbol) {
                Symbols.Add(symbol);
                return true;
            }

            return false;
        }

        internal void Close() {
            while (Symbols.Any() && !LastSymbolIsEvaluatable) {
                Symbols.RemoveAt(Symbols.Count - 1);
            }

            if (!Symbols.Any()) {
                Symbols.Add(new Number(0));
            }
        }
    }
}
