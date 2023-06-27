using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DecimalMathTests {
    [Theory]
    [MemberData(nameof(PowData))]
    public void Pow(decimal x, int y, decimal expectedResult) {
        Assert.Equal(expectedResult, DecimalMath.Pow(x, y));
    }

    public static TheoryData<decimal, int, decimal> PowData() => new() {
        { 10M, -5, 0.000_01M },
        { 10M, -2, 0.01M },
        { 10M, -1, 0.1M },
        { 10M, 0, 1M },
        { 10M, 1, 10M },
        { 10M, 2, 100M },
        { 10M, 5, 100_000M },
        { 2.5M, 2, 6.25M },
        { 2.5M, -2, 0.16M }
    };
}
