using System;

namespace BlazorPlayground.Calculator {
    internal class Pi : EvaluatableSymbol {
        internal override decimal Evaluate() => (decimal)Math.PI;

        public override string ToString() => "π";
    }
}
