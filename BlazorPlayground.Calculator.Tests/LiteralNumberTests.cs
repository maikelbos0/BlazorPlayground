using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class LiteralNumberTests {
        [Fact]
        public void LiteralNumber_Evaluate_Returns_Value() {
            var number = new LiteralNumber(12.3M);

            Assert.Equal(12.3M, number.Evaluate());
        }
    }
}
