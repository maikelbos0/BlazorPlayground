using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class SquareOperatorTests {
        [Fact]
        public void SquareOperator_Returns_Square_Of_Evaluated_Value() {
            var op = new SquareOperator() {
                Symbol = new LiteralNumber(5.5M)
            };

            Assert.Equal(30.25M, op.Evaluate());
        }

        [Fact]
        public void SquareOperator_ToString_Succeeds() {
            var op = new SquareOperator() {
                Symbol = new LiteralNumber(5.5M)
            };

            Assert.Equal("5.5²", op.ToString());
        }

        [Fact]
        public void SquareOperator_ToString_Adds_Parentheses_For_UnaryOperator_Symbol() {
            var op = new SquareOperator() {
                Symbol = new SquareOperator() {
                    Symbol = new LiteralNumber(5.5M)
                }
            };

            Assert.Equal("(5.5²)²", op.ToString());
        }
    }
}
