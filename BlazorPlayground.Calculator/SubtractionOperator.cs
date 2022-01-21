namespace BlazorPlayground.Calculator {
    internal class SubtractionOperator : BinaryOperator {
        internal SubtractionOperator(char character) : base(character) { }

        public override OperatorPrecedence Precedence => OperatorPrecedence.Low;

        public override decimal Invoke(decimal left, decimal right) => left - right;
    }
}
