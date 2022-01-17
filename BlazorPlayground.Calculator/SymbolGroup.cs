using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class SymbolGroup : IEvaluatableSymbol {
        public List<ISymbol> Symbols { get; } = new();
        public bool LastSymbolIsEvaluatable => Symbols.LastOrDefault() is IEvaluatableSymbol;

        public decimal Evaluate() {
            throw new System.NotImplementedException();
        }

        public bool Append(ISymbol symbol) {
            if (LastSymbolIsEvaluatable == symbol is not IEvaluatableSymbol) {
                Symbols.Add(symbol);
                return true;
            }

            return false;
        }

        public void Close() {
            while (Symbols.Any() && !LastSymbolIsEvaluatable) {
                Symbols.RemoveAt(Symbols.Count - 1);
            }

            if (!Symbols.Any()) {
                Symbols.Add(new Number(0));
            }
        }
    }
}
