using System.Globalization;

namespace BlazorPlayground.Calculator {
    internal class LiteralNumber : EvaluatableSymbol {
        private readonly decimal value;

        internal LiteralNumber(decimal value) {
            this.value = value;
        }

        internal override decimal Evaluate() => value;

        override public string ToString() => value.RemovePrecision().ToString(CultureInfo.InvariantCulture);
    }
}
