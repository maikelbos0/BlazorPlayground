using System.Collections.Generic;

namespace BlazorPlayground.Calculator {
    internal abstract class BinaryOperator : ISymbol {
        public BinaryOperator(char character) {
            Character = character;
        }

        internal char Character { get; }
        public abstract OperatorPrecedence Precedence { get; }

        public abstract decimal Invoke(decimal left, decimal right);

        public bool TryAppendTo(IList<ISymbol> symbols) {
            throw new System.NotImplementedException();
        }

        public override string ToString() => Character.ToString();
    }
}
