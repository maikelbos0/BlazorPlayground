using System.Collections.Generic;

namespace BlazorPlayground.Calculator {
    public class CalculationExpression {
        private readonly List<char> characters = new List<char>();

        public bool AcceptDigit => false;

        public bool AcceptDecimalSeparator => false;

        public bool AcceptBinaryOperator => false;

        public bool AcceptUnaryOperator => false;
        
        public bool AcceptOpeningParenthesis => false;
        
        public bool AcceptClosingParenthesis => false;

        public string Display => new string(characters.ToArray());

        public CalculationExpression() { }

        internal CalculationExpression(string value) => characters.AddRange(value);

        public bool TryAppend(char c) {
            characters.Add(c);

            return true;
        }

        public void Clear() {
            characters.Clear();
        }

        override public string ToString() => Display;
    }
}
