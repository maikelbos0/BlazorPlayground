using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class NegationOperatorTests {
        [Theory]
        [InlineData(-1.5, 1.5)]
        [InlineData(0, 0)]
        [InlineData(1.5, -1.5)]
        public void NegationOperator_Returns_Negative_Of_Evaluated_Value(decimal value, decimal expectedValue) {
            var op = new NegationOperator(new LiteralNumber(value));

            Assert.Equal(expectedValue, op.Evaluate());
        }
    }
}
