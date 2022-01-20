namespace BlazorPlayground.Calculator {
    internal interface IUnaryOperator : IEvaluatableSymbol { 
        IEvaluatableSymbol Symbol { get; }
    }
}
