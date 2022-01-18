using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class SubtractionOperatorTests {
        [Fact]
        public void SubtractionOperator_Invoke_Succeeds() {
            var op = new SubtractionOperator('-');

            Assert.Equal(15.5M, op.Invoke(19.4M, 3.9M));
        }

        [Fact]
        public void SubtractionOperator_ToString_Succeeds() {
            var op = new SubtractionOperator('?');

            Assert.Equal("?", op.ToString());
        }
    }
}
