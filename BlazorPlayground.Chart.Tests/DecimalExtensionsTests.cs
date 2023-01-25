using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DecimalExtensionsTests {
    [Theory]
    [InlineData(9, 1, 9)]
    [InlineData(9, 2, 8)]
    [InlineData(-9, 2, -10)]
    [InlineData(-0.1, 5, -5)]
    [InlineData(0.1, 5, 0)]
    [InlineData(119.9, 20, 100)]
    [InlineData(-100.1, 20, -120)]
    public void FloorToScale(decimal value, decimal scale, decimal expectedValue) {
        Assert.Equal(expectedValue, value.FloorToScale(scale));
    }
    [Theory]
    [InlineData(9, 1, 9)]
    [InlineData(9, 2, 10)]
    [InlineData(-9, 2, -8)]
    [InlineData(-0.1, 5, 0)]
    [InlineData(0.1, 5, 5)]
    [InlineData(100.1, 20, 120)]
    [InlineData(-119.9, 20, -100)]
    public void CeilingToScale(decimal value, decimal scale, decimal expectedValue) {
        Assert.Equal(expectedValue, value.CeilingToScale(scale));
    }
}