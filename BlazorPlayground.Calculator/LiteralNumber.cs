namespace BlazorPlayground.Calculator {
    internal class LiteralNumber : IEvaluatableSymbol {
        private readonly decimal value;

        public LiteralNumber(decimal value) {
            this.value = value;
        }

        public decimal Evaluate() {
            return value;
        }
    }
}
