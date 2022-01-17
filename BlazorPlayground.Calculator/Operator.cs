namespace BlazorPlayground.Calculator {
    internal abstract class Operator : ISymbol {
        // Precedence?
        public abstract decimal Invoke(decimal left, decimal right);
    }
}
