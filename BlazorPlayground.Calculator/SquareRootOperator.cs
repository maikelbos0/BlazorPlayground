using System;

namespace BlazorPlayground.Calculator {
    internal class SquareRootOperator : UnaryOperator {
        internal override decimal Evaluate() {
            var value = Symbol?.Evaluate() ?? throw new InvalidOperationException();

            return (decimal)Math.Sqrt((double)value);
        }

        public override string ToString() {
            if (Symbol is UnaryOperator) {
                return $"√({Symbol})";
            }
            else {
                return $"√{Symbol}";
            }
        }
    }
}
