using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class AbsoluteOperatorTests {
        [Theory]
        [InlineData(-5.5, 5.5)]
        [InlineData(5.5, 5.5)]
        public void AbsoluteOperator_Returns_Absolute_Of_Evaluated_Value(decimal value, decimal expectedValue) {
            var op = new AbsoluteOperator() {
                Symbol = new LiteralNumber(value)
            };

            Assert.Equal(expectedValue, op.Evaluate());
        }

        [Fact]
        public void SquareRootOperator_ToString_Succeeds() {
            var op = new AbsoluteOperator() {
                Symbol = new LiteralNumber(-5.5M)
            };

            Assert.Equal("|-5.5|", op.ToString());
        }
    }
}
