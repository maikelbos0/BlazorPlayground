using System.Globalization;

namespace BlazorPlayground.Calculator {
    internal class LiteralNumber : IEvaluatableSymbol {
        private readonly decimal value;

        internal LiteralNumber(decimal value) {
            this.value = value;
        }

        public decimal Evaluate() => value;

        override public string ToString() => value.RemovePrecision().ToString(CultureInfo.InvariantCulture);
    }
}
