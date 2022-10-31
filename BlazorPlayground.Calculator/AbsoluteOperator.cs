using System;

namespace BlazorPlayground.Calculator {
    internal class AbsoluteOperator : UnaryOperator {
        internal override decimal Evaluate() {
            return Math.Abs(Symbol?.Evaluate() ?? throw new InvalidOperationException());
        }

        public override string ToString() {
            return $"|{Symbol}|";
        }
    }
}
