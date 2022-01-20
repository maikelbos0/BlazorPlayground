namespace BlazorPlayground.Calculator {
    internal interface IBinaryOperator : ISymbol {
        public abstract OperatorPrecedence Precedence { get; }

        public abstract decimal Invoke(decimal left, decimal right);
    }
}
