using System;

namespace BlazorPlayground.Calculator {
    internal class SquareOperator : UnaryOperator {
        internal override decimal Evaluate() {
            var value = Symbol?.Evaluate() ?? throw new InvalidOperationException();

            return value * value;
        }

        public override string ToString() {
            if (Symbol is UnaryOperator) {
                return $"({Symbol})²";
            }
            else {
                return $"{Symbol}²";
            }
        }
    }
}
