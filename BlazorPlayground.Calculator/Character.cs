using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class Character : ISymbol {
        internal Character(char value) {
            Value = value;
        }

        internal char Value { get; }

        public bool TryAppendTo(IList<ISymbol> symbols) {
            var symbol = symbols.LastOrDefault();

            while (symbol is UnaryOperator op) {
                symbol = op.Symbol;
            }

            if (symbol is ComposableNumber number) {
                return number.TryAppend(Value);
            }

            number = new ComposableNumber();

            return number.TryAppend(Value) && number.TryAppendTo(symbols);
        }
    }
}
