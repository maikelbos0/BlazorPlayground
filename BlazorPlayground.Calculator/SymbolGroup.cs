using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class SymbolGroup : IEvaluatableSymbol {
        public List<ISymbol> Symbols { get; } = new();
        public bool LastSymbolIsEvaluatable => Symbols.LastOrDefault() is IEvaluatableSymbol;

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
                Symbols.Add(new LiteralNumber(0));
            }
        }

        public decimal Evaluate() {
            Close();

            var symbols = new List<ISymbol>(Symbols);

            foreach (var precedence in Enum.GetValues<OperatorPrecedence>().OrderBy(p => p)) {
                for (var i = 1; i < symbols.Count - 1; i += 2) {
                    var op = (Operator)symbols[i];

                    if (op.Precedence == precedence) {
                        var left = (IEvaluatableSymbol)symbols[i - 1];
                        var right = (IEvaluatableSymbol)symbols[i + 1];

                        symbols[i - 1] = new LiteralNumber(op.Invoke(left.Evaluate(), right.Evaluate()));
                        symbols.RemoveAt(i);
                        symbols.RemoveAt(i);
                        i -= 2;
                    }
                }
            }

            return ((IEvaluatableSymbol)symbols.Single()).Evaluate();
        }
    }
}
