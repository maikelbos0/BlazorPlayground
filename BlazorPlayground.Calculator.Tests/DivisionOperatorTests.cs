using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class DivisionOperatorTests {
        [Fact]
        public void DivisionOperator_Invoke_Succeeds() {
            var op = new DivisionOperator('/');

            Assert.Equal(3.1M, op.Invoke(16.12M, 5.2M));
        }

        [Fact]
        public void DivisionOperator_ToString_Succeeds() {
            var op = new DivisionOperator('?');

            Assert.Equal("?", op.ToString());
        }
    }
}
