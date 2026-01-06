using System.Collections.Generic;

namespace BlazorPlayground.Calculator;

internal abstract class EvaluatableSymbol : ISymbol {
    public virtual bool TryAppendTo(IList<ISymbol> symbols) {
        if (symbols.Count == 0 || symbols[^1] is BinaryOperator) {
            symbols.Add(this);
            return true;
        }

        return false;
    }

    internal abstract decimal Evaluate();
}
