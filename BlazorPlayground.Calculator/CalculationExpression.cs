using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    public class CalculationExpression {
        public CalculationExpression() {
            Groups = new();
            Groups.Push(new());
        }

        internal Stack<SymbolGroup> Groups { get; }
        internal SymbolGroup CurrentGroup => Groups.Peek();

        public bool TryAppend(char c) {
            if (c == '(') {
                return OpenGroup();
            }
            else if (c == ')') {
                return CloseGroup();
            }
            else if (TryAppendBinaryOperator(c)) {
                return true;
            }
            else if (TryAppendUnaryOperator(c)) {
                return true;
            }
            else if (TryAppendDigit(c)) {
                return true;
            }
            else {
                var number = new ComposableNumber();

                return number.TryAppend(c) && CurrentGroup.TryAppend(number);
            }

            bool TryAppendBinaryOperator(char c) {
                var op = BinaryOperatorFactory.GetOperator(c);

                return op != null && CurrentGroup.TryAppend(op);
            }

            bool TryAppendUnaryOperator(char c) {
                if (CurrentGroup.Symbols.LastOrDefault() is EvaluatableSymbol symbol) {
                    var op = UnaryOperatorFactory.GetOperator(c);

                    return op != null && CurrentGroup.TryAppend(op);
                }

                return false;
            }

            bool TryAppendDigit(char c) {
                return CurrentGroup.Symbols.LastOrDefault() is ComposableNumber number && number.TryAppend(c);
            }
        }

        internal bool OpenGroup() {
            var group = new SymbolGroup();
            var success = CurrentGroup.TryAppend(group);

            if (success) {
                Groups.Push(group);
            }

            return success;
        }

        internal bool CloseGroup() {
            if (Groups.Count == 1) {
                return false;
            }

            Groups.Pop().Close();

            return true;
        }

        public void Clear() {
            Groups.Clear();
            Groups.Push(new());
        }

        public decimal Evaluate() {
            while (CloseGroup()) { }

            var value = Groups.Pop().Evaluate();

            Groups.Push(new());
            CurrentGroup.TryAppend(new LiteralNumber(value));

            return value;
        }

        override public string ToString() {
            var value = Groups.Last().ToString();
            
            return value.Substring(1, value.Length - 1 - Groups.Count);
        }
    }
}
