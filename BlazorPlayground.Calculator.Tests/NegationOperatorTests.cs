using System.Collections.Generic;
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

            Assert.Equal("-5.5", op.ToString());
        }

        [Fact]
        public void NegationOperator_ToString_Adds_Parentheses_For_UnaryOperator_Symbol() {
            var op = new NegationOperator() {
                Symbol = new SquareOperator() {
                    Symbol = new LiteralNumber(5.5M)
                }
            };

            Assert.Equal("-(5.5²)", op.ToString());
        }

        [Fact]
        public void NegationOperator_TryAppendTo_Replaces_Symbol_For_Double_Negation() {
            var op = new NegationOperator();
            var number = new LiteralNumber(5.5M);
            var symbols = new List<ISymbol>() {
                new NegationOperator() {
                    Symbol = number
                }
            };

            Assert.True(op.TryAppendTo(symbols));
            Assert.Equal(number, Assert.Single(symbols));
        }

        [Fact]
        public void NegationOperator_TryAppendTo_Negates__LiteralNumber_Value_When_Negation() {
            var op = new NegationOperator();
            var symbols = new List<ISymbol>() {
                new LiteralNumber(-5.5M)
            };

            Assert.True(op.TryAppendTo(symbols));
            Assert.Equal(5.5M, Assert.IsType<LiteralNumber>(Assert.Single(symbols)).Value);
        }

        [Fact]
        public void UnaryOperator_TryAppendTo_Succeeds_When_Previous_Symbol_Is_Evaluatable() {
            var op = new NegationOperator();
            var number = new LiteralNumber(5.5M);
            var symbols = new List<ISymbol>() {
                number
            };

            Assert.True(op.TryAppendTo(symbols));
            Assert.Equal(op, Assert.Single(symbols));
            Assert.Equal(number, op.Symbol);
        }
    }
}
