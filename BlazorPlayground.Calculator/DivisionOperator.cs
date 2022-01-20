namespace BlazorPlayground.Calculator {
    internal class DivisionOperator : IOperator {
        internal DivisionOperator(char character) {
            Character = character;
        }

        public OperatorPrecedence Precedence => OperatorPrecedence.High;

        internal char Character { get; }

        public decimal Invoke(decimal left, decimal right) => left / right;

        public override string ToString() => Character.ToString();
    }
}
