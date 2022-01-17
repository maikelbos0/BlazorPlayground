using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public CalculationExpression() {
            groups = new();
            groups.Push(new());
        }

        internal CalculationExpression(string value) : this() => characters.AddRange(value);

        public bool TryAppend(char c) {
            characters.Add(c);

            return true;
        }

        public void Clear() {
            characters.Clear();
        }

        override public string ToString() => Display;

        private readonly Stack<SymbolGroup> groups;
        private StringBuilder? numberBuilder;

        private SymbolGroup CurrentGroup => groups.Peek();

        internal bool Append(ISymbol symbol) => CurrentGroup.Append(symbol);

        internal bool OpenGroup() {
            var group = new SymbolGroup();
            var success = CurrentGroup.Append(group);

            if (success) {
                groups.Push(group);
            }

            return success;
        }

        internal bool CloseGroup() {
            if (groups.Count == 1) {
                return false;
            }

            groups.Pop().Close();

            return true;
        }
    }
}
