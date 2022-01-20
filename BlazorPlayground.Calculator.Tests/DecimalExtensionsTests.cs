using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class DecimalExtensionsTests {
        [Fact]
        public void Decimal_RemovePrecision_Removes_Trailing_Precision_Zeroes() {
            var value = 1.250000M;

            Assert.Equal(1.25M.ToString(), value.RemovePrecision().ToString());
        }
    }
}
