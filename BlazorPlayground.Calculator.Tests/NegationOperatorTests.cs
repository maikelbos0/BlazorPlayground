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
    }
}
