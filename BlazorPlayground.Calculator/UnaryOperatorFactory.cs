namespace BlazorPlayground.Calculator {
    internal static class UnaryOperatorFactory {
        internal static IEvaluatableSymbol? GetOperator(char character, IEvaluatableSymbol symbol) => character switch {
            '±' => new NegationOperator(symbol),
            _ => null
        };
    }
}
