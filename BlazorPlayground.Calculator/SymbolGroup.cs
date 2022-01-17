using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class SymbolGroup : IEvaluatableSymbol {
        internal List<ISymbol> Symbols { get; } = new();

        internal bool Append(ISymbol symbol) {
            if ((!Symbols.Any() || Symbols.Last() is not IEvaluatableSymbol) && symbol is IEvaluatableSymbol) {
                Symbols.Add(symbol);
                return true;
            }

            if (Symbols.LastOrDefault() is IEvaluatableSymbol && symbol is not IEvaluatableSymbol) {
                Symbols.Add(symbol);
                return true;
            }

            return false;
        }

        internal void Close() {
            while (Symbols.Any() && Symbols.Last() is not IEvaluatableSymbol) {
                Symbols.RemoveAt(Symbols.Count - 1);
            }

            if (!Symbols.Any()) {
                Symbols.Add(new Number(0));
            }
        }
    }
}
