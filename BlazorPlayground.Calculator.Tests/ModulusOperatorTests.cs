using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class ModulusOperatorTests {
        [Fact]
        public void ModulusOperator_Invoke_Succeeds() {
            var op = new ModulusOperator('%');

            Assert.Equal(0.15M, op.Invoke(16.65M, 5.5M));
        }

        [Fact]
        public void ModulusOperator_ToString_Succeeds() {
            var op = new ModulusOperator('?');

            Assert.Equal("mod", op.ToString());
        }
    }
}
