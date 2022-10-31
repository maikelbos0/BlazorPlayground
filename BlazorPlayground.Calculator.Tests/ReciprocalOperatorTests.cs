using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class ReciprocalOperatorTests {
        [Theory]
        [InlineData(10, 0.1)]
        [InlineData(0.1, 10)]
        public void ReciprocalOperator_Returns_Multiplicative_Inverse(decimal value, decimal expectedValue) {
            var op = new ReciprocalOperator() {
                Symbol = new LiteralNumber(value)
            };

            Assert.Equal(expectedValue, op.Evaluate());
        }

        [Fact]
        public void ReciprocalOperator_ToString_Succeeds() {
            var op = new ReciprocalOperator() {
                Symbol = new LiteralNumber(5.5M)
            };

            Assert.Equal("5.5⁻¹", op.ToString());
        }

        [Fact]
        public void ReciprocalOperator_ToString_Adds_Parentheses_For_UnaryOperator_Symbol() {
            var op = new ReciprocalOperator() {
                Symbol = new SquareOperator() {
                    Symbol = new LiteralNumber(5.5M)
                }
            };

            Assert.Equal("(5.5²)⁻¹", op.ToString());
        }
    }
}
