using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class SquareRootOperatorTests {
        [Fact]
        public void SquareRootOperator_Returns_Square_Root_Of_Evaluated_Value() {
            var op = new SquareRootOperator() {
                Symbol = new LiteralNumber(30.25M)
            };

            Assert.Equal(5.5M, op.Evaluate());
        }

        [Fact]
        public void SquareRootOperator_ToString_Succeeds() {
            var op = new SquareRootOperator() {
                Symbol = new LiteralNumber(5.5M)
            };

            Assert.Equal("√5.5", op.ToString());
        }

        [Fact]
        public void SquareRootOperator_ToString_Adds_Parentheses_For_UnaryOperator_Symbol() {
            var op = new SquareRootOperator() {
                Symbol = new SquareOperator() {
                    Symbol = new LiteralNumber(5.5M)
                }
            };

            Assert.Equal("√(5.5²)", op.ToString());
        }
    }
}
