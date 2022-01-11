using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    public class CalculationExpression {
        private const char openingParenthesis = '(';
        private const char closingParenthesis = ')';
        private static readonly HashSet<char> unaryOperator = new HashSet<char>() { '±' };
        private static readonly HashSet<char> digits = new HashSet<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static readonly HashSet<char> decimalSeparators = new HashSet<char>() { '.', ',' };
        private static readonly HashSet<char> binaryOperators = new HashSet<char>() { '/', '÷', '*', '×', '-', '−', '+' };

        private readonly List<char> characters = new List<char>();

        public bool AcceptDigit => !characters.Any() || characters.Last() != closingParenthesis;

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
