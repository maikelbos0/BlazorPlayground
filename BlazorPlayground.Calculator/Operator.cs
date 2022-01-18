namespace BlazorPlayground.Calculator {
    internal abstract class Operator : ISymbol {
        public Operator(char character) {
            Character = character;
        }

        public abstract OperatorPrecedence Precedence { get; }
        public char Character { get; }

        public abstract decimal Invoke(decimal left, decimal right);
        public override string ToString() {
            return Character.ToString();
        }
    }
}
