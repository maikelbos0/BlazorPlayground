using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DecimalMathTests {
    [Theory]
    [MemberData(nameof(PowData))]
    public void Pow(decimal x, int y, decimal expectedResult) {
        Assert.Equal(expectedResult, DecimalMath.Pow(x, y));
    }

    public static TheoryData<decimal, int, decimal> PowData() => new() {
        { 10M, -15, 0.000_000_000_000_001M },
        { 10M, -5, 0.000_01M },
        { 10M, -2, 0.01M },
        { 10M, -1, 0.1M },
        { 10M, 0, 1M },
        { 10M, 1, 10M },
        { 10M, 2, 100M },
        { 10M, 5, 100_000M },
        { 10M, 15, 1_000_000_000_000_000M },
        { 2.5M, 2, 6.25M },
        { 2.5M, -2, 0.16M }
    };

    [Theory]
    [MemberData(nameof(FloorToScaleData))]
    public void FloorToScale(decimal value, decimal scale, decimal expectedValue) {
        Assert.Equal(expectedValue, DecimalMath.FloorToScale(value, scale));
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
        Assert.Equal(expectedValue, DecimalMath.CeilingToScale(value, scale));
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

    [Theory]
    [MemberData(nameof(TrimData))]
    public void Trim(decimal value, decimal expectedValue) {
        Assert.Equal(expectedValue, DecimalMath.Trim(value));
    }

    public static TheoryData<decimal, decimal> TrimData() => new() {
        { 0.250_000_000_000_000_000_000_000_000_000M, 0.25M },
        { 0.250_000M, 0.25M },
        { 0.25M, 0.25M },
        { 10.000_000_000_000_000_000_000_000_000_000M, 10M },
        { 10.000_000M, 10M },
        { 10M, 10M }
    };

    [Theory]
    [MemberData(nameof(AdjustToRangeData))]
    public void AdjustToRange(decimal value, decimal min, decimal max, decimal expectedValue) {
        Assert.Equal(expectedValue, DecimalMath.AdjustToRange(value, min, max));
    }

    public static TheoryData<decimal, decimal, decimal, decimal> AdjustToRangeData() => new() {
        { -2M, -1M, 3M, -1M },
        { -1M, -1M, 3M, -1M },
        { 0M, -1M, 3M, 0M },
        { 1M, -1M, 3M, 1M },
        { 2M, -1M, 3M, 2M },
        { 3M, -1M, 3M, 3M },
        { 4M, -1M, 3M, 3M }
    };
}
