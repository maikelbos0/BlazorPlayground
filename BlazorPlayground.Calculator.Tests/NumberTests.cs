using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class NumberTests {
        [Fact]
        public void Number_Evaluate_Returns_Value() {
            var number = new Number(12.3M);

            Assert.Equal(12.3M, number.Evaluate());
        }
    }
}
