using System.Collections.Generic;

namespace BlazorPlayground.Calculator {
    internal abstract class EvaluatableSymbol : ISymbol {
        public virtual bool TryAppendTo(IList<ISymbol> symbols) {
            throw new System.NotImplementedException();
        }

        internal abstract decimal Evaluate();
    }
}
