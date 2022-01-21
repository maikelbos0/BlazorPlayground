namespace BlazorPlayground.Calculator {
    internal static class UnaryOperatorFactory {
        internal static UnaryOperator? GetOperator(char character) => character switch {
            '±' => new NegationOperator(),
            _ => null
        };
    }
}
