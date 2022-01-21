using System;

namespace BlazorPlayground.Calculator {
    internal class NegationOperator : UnaryOperator {
        internal override decimal Evaluate() {
            return -Symbol?.Evaluate() ?? throw new InvalidOperationException();
        }

        public override string ToString() => $"-({Symbol})";
    }
}
