using System.Collections.Generic;

namespace BlazorPlayground.Calculator;

internal abstract class BinaryOperator : ISymbol {
    public BinaryOperator(char character) {
        Character = character;
    }

    internal char Character { get; }
    public abstract OperatorPrecedence Precedence { get; }

    public abstract decimal Invoke(decimal left, decimal right);

    public bool TryAppendTo(IList<ISymbol> symbols) {
        if (symbols.Count == 0) {
            symbols.Add(new ComposableNumber());
        }

        if (symbols[^1] is BinaryOperator) {
            symbols[^1] = this;
        }
        else {
            symbols.Add(this);
        }

        return true;
    }

    public override string ToString() => Character.ToString();
}
