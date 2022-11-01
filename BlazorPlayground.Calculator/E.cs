using System;

namespace BlazorPlayground.Calculator {
    internal class E : EvaluatableSymbol {
        internal override decimal Evaluate() => (decimal)Math.E;

        public override string ToString() => "e";
    }
}
