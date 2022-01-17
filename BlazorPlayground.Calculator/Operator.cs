namespace BlazorPlayground.Calculator {
    internal abstract class Operator : ISymbol {
        public abstract OperatorPrecedence Precedence { get; }
        public abstract decimal Invoke(decimal left, decimal right);
    }
}
