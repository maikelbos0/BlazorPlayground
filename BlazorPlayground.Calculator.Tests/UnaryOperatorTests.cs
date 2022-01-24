using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class UnaryOperatorTests {
        [Fact]
        public void UnaryOperator_TryAppendTo_Succeeds_When_Previous_Symbol_Is_Evaluatable() {
            var op = new SquareOperator();
            var number = new LiteralNumber(5.5M);
            var symbols = new List<ISymbol>() {
                number
            };

            Assert.True(op.TryAppendTo(symbols));
            Assert.Equal(op, Assert.Single(symbols));
            Assert.Equal(number, op.Symbol);
        }

        [Fact]
        public void UnaryOperator_TryAppendTo_Fails_When_Symbols_Is_Empty() {
            var op = new SquareOperator();
            var symbols = new List<ISymbol>();

            Assert.False(op.TryAppendTo(symbols));
            Assert.Empty(symbols);
            Assert.Null(op.Symbol);
        }

        [Fact]
        public void UnaryOperator_TryAppendTo_Fails_When_Previous_Symbol_Is_Not_Evaluatable() {
            var op = new SquareOperator();
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
