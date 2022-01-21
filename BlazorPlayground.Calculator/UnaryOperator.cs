using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal abstract class UnaryOperator : EvaluatableSymbol {
        internal EvaluatableSymbol? Symbol { get; set; }

        internal override abstract decimal Evaluate();

        public override bool TryAppendTo(IList<ISymbol> symbols) {
            if (symbols.Count > 0 && symbols.Last() is EvaluatableSymbol symbol) {
                Symbol = symbol;
                symbols[^1] = this;
                return true;
            }

            return false;
        }
    }
}
