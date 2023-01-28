using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class YAxisTests {

    [Theory]
    [InlineData(1.0, 0.0, 5.0)]
    [InlineData(1.0, 100.5, 105.5, 103.0)]
    // TODO add more tests
    public void AutoScale(double expectedGridLineInterval, double expectedMin, double expectedMax, params double[] dataPoints) {
        var result = YAxis.AutoScale(dataPoints);

        Assert.Equal(expectedGridLineInterval, result.GridLineInterval);
        Assert.Equal(expectedMin, result.Min);
        Assert.Equal(expectedMax, result.Max);
    }

    [Fact]
    public void GridLines() {
        var subject = new YAxis(20, -10, 105);

        Assert.Equal(new double[] { 0, 20, 40, 60, 80, 100 }, subject.GetGridLines());
    }
}
