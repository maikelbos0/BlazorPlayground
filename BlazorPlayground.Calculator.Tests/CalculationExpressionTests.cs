using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class CalculationExpressionTests {
        [Theory]
        [InlineData("", true)]
        [InlineData("1", true)]
        [InlineData("1.", true)]
        [InlineData("1.2", true)]
        [InlineData("1+", true)]
        [InlineData("1+2", true)]
        [InlineData("(", true)]
        [InlineData("(1", true)]
        [InlineData("(1)", false)]
        [InlineData("-", true)]
        public void CalculationExpression_AcceptDigit(string value, bool expected) {
            var expression = new CalculationExpression(value);

            Assert.Equal(expected, expression.AcceptDigit);
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("1", true)]
        [InlineData("1.", false)]
        [InlineData("1.1", false)]
        [InlineData("1.1+", true)]
        [InlineData("(", true)]
        [InlineData("(1", true)]
        [InlineData("(1)", false)]
        [InlineData("-", true)]
        public void CalculationExpression_AcceptDecimalSeparator(string value, bool expected) {
            var expression = new CalculationExpression(value);

            Assert.Equal(expected, expression.AcceptDecimalSeparator);
        }
    }
}
