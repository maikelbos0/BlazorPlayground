namespace BlazorPlayground.Calculator {
    internal interface IEvaluatableSymbol : ISymbol {
        decimal Evaluate();
    }
}
