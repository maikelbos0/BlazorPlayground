namespace BlazorPlayground.Calculator {
    internal class ModulusOperator : BinaryOperator {
        public ModulusOperator(char character) : base(character) {
        }

        public override OperatorPrecedence Precedence => OperatorPrecedence.High;

        public override decimal Invoke(decimal left, decimal right) => left % right;

        public override string ToString() {
            return "mod";
        }
    }
}
