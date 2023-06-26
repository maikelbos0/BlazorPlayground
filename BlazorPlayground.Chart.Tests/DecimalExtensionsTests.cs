using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DecimalExtensionsTests {
    [Theory]
    [MemberData(nameof(FloorToScaleData))]
    public void FloorToScale(decimal value, decimal scale, decimal expectedValue) {
        Assert.Equal(expectedValue, value.FloorToScale(scale));
    }

    public static TheoryData<decimal, decimal, decimal> FloorToScaleData() => new() {
        { 9M, 1M, 9M },
        { 9M, 2M, 8M },
        { -9M, 2M, -10M },
        { -0.1M, 5M, -5M },
        { 0.1M, 5M, 0M },
        { 119.9M, 20M, 100M },
        { -100.1M, 20M, -120M }
    };

    [Theory]
    [MemberData(nameof(CeilingToScaleData))]
    public void CeilingToScale(decimal value, decimal scale, decimal expectedValue) {
        Assert.Equal(expectedValue, value.CeilingToScale(scale));
    }

    public static TheoryData<decimal, decimal, decimal> CeilingToScaleData() => new() {
        { 9M, 1M, 9M },
        { 9M, 2M, 10M },
        { -9M, 2M, -8M },
        { -0.1M, 5M, 0M },
        { 0.1M, 5M, 5M },
        { 100.1M, 20M, 120M },
        { -119.9M, 20M, -100M }
    };
}
