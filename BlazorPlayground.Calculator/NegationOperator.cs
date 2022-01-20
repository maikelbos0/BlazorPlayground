namespace BlazorPlayground.Calculator {
    internal class NegationOperator : IEvaluatableSymbol {
        public NegationOperator(IEvaluatableSymbol symbol) {
            Symbol = symbol;
        }

        public IEvaluatableSymbol Symbol { get; }

        public decimal Evaluate() {
            return -Symbol.Evaluate();
        }
    }
}
