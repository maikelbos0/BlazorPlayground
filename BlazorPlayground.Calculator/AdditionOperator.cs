namespace BlazorPlayground.Calculator {
    internal class AdditionOperator : IBinaryOperator {
        internal AdditionOperator(char character) {
            Character = character;
        }

        public OperatorPrecedence Precedence => OperatorPrecedence.Low;

        internal char Character { get; }

        public decimal Invoke(decimal left, decimal right) => left + right;

        public override string ToString() => Character.ToString();
    }
}
