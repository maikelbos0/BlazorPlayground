﻿using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Calculator {
    internal class Backspace : ISymbol {
        public bool TryAppendTo(IList<ISymbol> symbols) {
            var symbol = symbols.LastOrDefault();

            while (symbol is UnaryOperator op) {
                symbol = op.Symbol;
            }

            if (symbol is ComposableNumber number) {
                return number.TryRemoveCharacter();
            }

            return false;
        }
    }
}
