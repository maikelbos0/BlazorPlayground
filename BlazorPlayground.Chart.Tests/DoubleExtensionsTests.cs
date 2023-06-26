using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DoubleExtensionsTests {
    [Theory]
    [InlineData(9, 1, 9)]
    [InlineData(9, 2, 8)]
    [InlineData(-9, 2, -10)]
    [InlineData(-0.1, 5, -5)]
    [InlineData(0.1, 5, 0)]
    [InlineData(119.9, 20, 100)]
    [InlineData(-100.1, 20, -120)]
    public void FloorToScale(double value, double scale, double expectedValue) {
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
    public void CeilingToScale(double value, double scale, double expectedValue) {
        Assert.Equal(expectedValue, value.CeilingToScale(scale));
    }

    [Theory]
    [InlineData(0.01, 0.79, 0.1)]
    [InlineData(0.1, 7.9, 1)]
    [InlineData(0, 8, 1)]
    [InlineData(0, 8.1, 2)]
    [InlineData(1, 15, 2)]
    [InlineData(0, 16, 2)]
    [InlineData(-0.1, 16, 5)]
    [InlineData(1, 39, 5)]
    [InlineData(0, 40, 5)]
    [InlineData(0, 41, 10)]
    [InlineData(-1, 80, 20)]
    public void GetScale(double min, double max, double expectedScale) {
        Assert.Equal(expectedScale, DoubleExtensions.GetScale(min, max));
    }
}
