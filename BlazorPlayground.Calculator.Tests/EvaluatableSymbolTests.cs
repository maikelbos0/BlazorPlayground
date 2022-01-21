using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class EvaluatableSymbolTests {
        [Fact]
        public void EvaluableSymbol_TryAppendTo_Succeeds_When_Symbols_Is_Empty() {
            var number = new LiteralNumber(0);
            var symbols = new List<ISymbol>();

            Assert.True(number.TryAppendTo(symbols));
            Assert.Equal(number, Assert.Single(symbols));
        }

        [Fact]
        public void EvaluableSymbol_TryAppendTo_Succeeds_When_Last_Symbol_Is_BinaryOperator() {
            var number = new LiteralNumber(0);
            var symbols = new List<ISymbol>() {
                new LiteralNumber(1),
                new AdditionOperator('+')
            };

            Assert.True(number.TryAppendTo(symbols));
            Assert.Equal(3, symbols.Count);
            Assert.Equal(number, symbols[2]);
        }

        [Fact]
        public void EvaluableSymbol_TryAppendTo_Fails_When_Last_Symbol_Is_EvaluatableSymbol() {
            var number = new LiteralNumber(1);
            var symbols = new List<ISymbol>() {
                new LiteralNumber(1)
            };

            Assert.False(number.TryAppendTo(symbols));
            Assert.NotEqual(number, Assert.Single(symbols));
        }
    }
}
