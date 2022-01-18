using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class MultiplicationOperatorTests {
        [Fact]
        public void MultiplicationOperator_Invoke_Succeeds() {
            var op = new MultiplicationOperator('*');

            Assert.Equal(16.12M, op.Invoke(5.2M, 3.1M));
        }

        [Fact]
        public void MultiplicationOperator_ToString_Succeeds() {
            var op = new MultiplicationOperator('?');

            Assert.Equal("?", op.ToString());
        }
    }
}
