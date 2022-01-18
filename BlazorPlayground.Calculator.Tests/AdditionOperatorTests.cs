using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class AdditionOperatorTests {
        [Fact]
        public void AdditionOperator_Invoke_Succeeds() {
            var op = new AdditionOperator('+');

            Assert.Equal(15.5M, op.Invoke(9.4M, 6.1M));
        }

        [Fact]
        public void AdditionOperator_ToString_Succeeds() {
            var op = new AdditionOperator('?');

            Assert.Equal("?", op.ToString());
        }
    }
}
