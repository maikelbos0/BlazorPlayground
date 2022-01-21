using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class SymbolGroup : EvaluatableSymbol {
        internal List<ISymbol> Symbols { get; } = new();

        internal bool TryAppend(ISymbol symbol) => symbol.TryAppendTo(Symbols);

        internal void Close() {
            while (Symbols.Count > 0 && Symbols.Last() is BinaryOperator) {
                Symbols.RemoveAt(Symbols.Count - 1);
            }

            if (!Symbols.Any()) {
                Symbols.Add(new LiteralNumber(0));
            }
        }

        internal override decimal Evaluate() {
            Close();

            var symbols = new List<ISymbol>(Symbols);

            foreach (var precedence in Enum.GetValues<OperatorPrecedence>().OrderBy(p => p)) {
                for (var i = 1; i < symbols.Count - 1; i += 2) {
                    var op = (BinaryOperator)symbols[i];

                    if (op.Precedence == precedence) {
                        var left = (EvaluatableSymbol)symbols[i - 1];
                        var right = (EvaluatableSymbol)symbols[i + 1];

                        symbols[i - 1] = new LiteralNumber(op.Invoke(left.Evaluate(), right.Evaluate()));
                        symbols.RemoveAt(i);
                        symbols.RemoveAt(i);
                        i -= 2;
                    }
                }
            }

            return ((EvaluatableSymbol)symbols.Single()).Evaluate();
        }

        public override string ToString() => $"({string.Join(' ', Symbols)})";
    }
}
