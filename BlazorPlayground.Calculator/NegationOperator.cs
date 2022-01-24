using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class NegationOperator : UnaryOperator {
        internal override decimal Evaluate() {
            return -Symbol?.Evaluate() ?? throw new InvalidOperationException();
        }

        public override bool TryAppendTo(IList<ISymbol> symbols) {
            if (symbols.LastOrDefault() is NegationOperator op && op.Symbol != null) {
                symbols[^1] = op.Symbol;
                return true;
            }

            if (symbols.LastOrDefault() is LiteralNumber number && number.Value < 0) {
                number.Value = -number.Value;
                return true;
            }

            return base.TryAppendTo(symbols);
        }

        public override string ToString() {
            if (Symbol is UnaryOperator) {
                return $"-({Symbol})";
            }
            else {
                return $"-{Symbol}";
            }
        }
    }
}
