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
            else if (CurrentGroup.LastSymbol is ComposableNumber number && number.Append(c)) {
                return true;
            }

            var op = OperatorFactory.GetOperator(c);

            if (op != null && CurrentGroup.Append(op)) {
                return true;
            }
            else {
                var number = new ComposableNumber();

                return number.Append(c) && CurrentGroup.Append(number);
            }
        }

        internal bool OpenGroup() {
            var group = new SymbolGroup();
            var success = CurrentGroup.Append(group);

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
            CurrentGroup.Append(new LiteralNumber(value));

            return value;
        }

        override public string ToString() {
            var value = Groups.Last().ToString();
            
            return value.Substring(1, value.Length - 1 - Groups.Count);
        }
    }
}
