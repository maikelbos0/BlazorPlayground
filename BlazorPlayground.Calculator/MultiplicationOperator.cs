namespace BlazorPlayground.Calculator {
    internal class MultiplicationOperator : Operator {
        public MultiplicationOperator(char character) : base(character) { }

        public override OperatorPrecedence Precedence => OperatorPrecedence.High;

        public override decimal Invoke(decimal left, decimal right) => left * right;
    }
}
