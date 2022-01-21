using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class NegationOperatorTests {
        [Theory]
        [InlineData(-1.5, 1.5)]
        [InlineData(0, 0)]
        [InlineData(1.5, -1.5)]
        public void NegationOperator_Returns_Negative_Of_Evaluated_Value(decimal value, decimal expectedValue) {
            var op = new NegationOperator() {
                Symbol = new LiteralNumber(value)
            };

            Assert.Equal(expectedValue, op.Evaluate());
        }

        [Fact]
        public void NegationOperator_ToString_Succeeds() {
            var op = new NegationOperator() {
                Symbol = new LiteralNumber(5.5M)
            };

            Assert.Equal("-(5.5)", op.ToString());
        }

        [Fact]
        public void NegationOperator_TryAppendTo_Succeeds_When_Previous_Symbol_Is_Evaluatable() {
            var op = new NegationOperator();
            var number = new LiteralNumber(5.5M);
            var symbols = new List<ISymbol>() {
                number
            };

            Assert.True(op.TryAppendTo(symbols));
            Assert.Equal(op, Assert.Single(symbols));
            Assert.Equal(number, op.Symbol);
        }

        [Fact]
        public void NegationOperator_TryAppendTo_Fails_When_Symbols_Is_Empty() {
            var op = new NegationOperator();
            var symbols = new List<ISymbol>();

            Assert.False(op.TryAppendTo(symbols));
            Assert.Empty(symbols);
            Assert.Null(op.Symbol);
        }

        [Fact]
        public void NegationOperator_TryAppendTo_Fails_When_Previous_Symbol_Is_Not_Evaluatable() {
            var op = new NegationOperator();
            var binaryOp = new AdditionOperator('+');
            var symbols = new List<ISymbol>() {
                new LiteralNumber(5.5M),
                binaryOp
            };

            Assert.False(op.TryAppendTo(symbols));
            Assert.Equal(2, symbols.Count);
            Assert.Equal(binaryOp, symbols.Last());
            Assert.Null(op.Symbol);
        }
    }
}
