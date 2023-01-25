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
    
    [Theory]
    [InlineData(0.05, 0.45, 0.1)]
    [InlineData(0.5, 4.5, 1)]
    [InlineData(0, 5, 1)]
    [InlineData(0, 5.1, 2)]
    [InlineData(1, 9, 2)]
    [InlineData(0, 10, 2)]
    [InlineData(-0.1, 10, 5)]
    [InlineData(1, 24, 5)]
    [InlineData(0, 25, 5)]
    [InlineData(0, 26, 10)]
    public void GetScale(decimal min, decimal max, decimal expectedScale) {
        Assert.Equal(expectedScale, DecimalExtensions.GetScale(min, max));
    }
}