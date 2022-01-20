namespace BlazorPlayground.Calculator {
    internal class NegationOperator : IUnaryOperator {
        internal NegationOperator(IEvaluatableSymbol symbol) {
            Symbol = symbol;
        }

        public IEvaluatableSymbol Symbol { get; }

        public decimal Evaluate() {
            return -Symbol.Evaluate();
        }

        public override string ToString() => $"-({Symbol})";
    }
}
