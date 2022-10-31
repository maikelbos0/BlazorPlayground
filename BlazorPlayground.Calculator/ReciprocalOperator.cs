using System;

namespace BlazorPlayground.Calculator {
    internal class ReciprocalOperator : UnaryOperator {
        internal override decimal Evaluate() {
            return 1M / Symbol?.Evaluate() ?? throw new InvalidOperationException();
        }

        public override string ToString() {
            if (Symbol is UnaryOperator) {
                return $"({Symbol})⁻¹";
            }
            else {
                return $"{Symbol}⁻¹";
            }
        }
    }
}
