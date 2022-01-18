namespace BlazorPlayground.Calculator {
    internal class AdditionOperator : Operator {
        public AdditionOperator(char character) : base(character) { }

        public override OperatorPrecedence Precedence => OperatorPrecedence.Low;


        public override decimal Invoke(decimal left, decimal right) => left + right;
    }
}
