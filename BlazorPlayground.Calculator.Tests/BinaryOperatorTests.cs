using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class BinaryOperatorTests {
        [Fact]
        public void BinaryOperator_TryAppendTo_Succeeds_When_Symbols_Is_Empty() {
            var op = new AdditionOperator('+');
            var symbols = new List<ISymbol>();

            Assert.True(op.TryAppendTo(symbols));
            Assert.Equal(2, symbols.Count);
            Assert.Equal(op, symbols[1]);
            Assert.Equal(0, Assert.IsType<LiteralNumber>(symbols[0]).Value);
        }

        [Fact]
        public void BinaryOperator_TryAppendTo_Replaces_Last_Symbol_If_BinaryOperator() {
            var op = new AdditionOperator('+');
            var symbols = new List<ISymbol>() {
                new LiteralNumber(1),
                new SubtractionOperator('-')
            };

            Assert.True(op.TryAppendTo(symbols));
            Assert.Equal(2, symbols.Count);
            Assert.Equal(op, symbols[1]);
        }

        [Fact]
        public void BinaryOperator_TryAppendTo_Succeeds_When_Last_Symbol_Is_EvaluatableSymbol() {
            var op = new AdditionOperator('+');
            var symbols = new List<ISymbol>() {
                new LiteralNumber(1)
            };

            Assert.True(op.TryAppendTo(symbols));
            Assert.Equal(2, symbols.Count);
            Assert.Equal(op, symbols[1]);
        }
    }
}
