using System.Globalization;

namespace BlazorPlayground.Calculator {
    internal class LiteralNumber : EvaluatableSymbol {
        internal decimal Value { get; set; }

        internal LiteralNumber(decimal value) {
            Value = value;
        }

        internal override decimal Evaluate() => Value;

        override public string ToString() => Value.RemovePrecision().ToString(CultureInfo.InvariantCulture);
    }
}
