namespace BlazorPlayground.Calculator {
    internal class NegationOperator : IEvaluatableSymbol {
        internal NegationOperator(IEvaluatableSymbol symbol) {
            Symbol = symbol;
        }

        internal IEvaluatableSymbol Symbol { get; }

        public decimal Evaluate() {
            return -Symbol.Evaluate();
        }

        public override string ToString() => $"-{Symbol}";
    }
}
