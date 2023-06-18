using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class YAxisTests {
    [Theory]
    [InlineData(1.0, 0.0, 5.0)]
    [InlineData(1.0, 100.0, 106.0, 103.0)]
    [InlineData(1.0, 0.0, 5.0, 0.2, 4.8)]
    [InlineData(2.0, -2.0, 10.0, -0.2, 8.2)]
    [InlineData(0.1, 0.0, 0.8, 0.01, 0.79)]
    [InlineData(1.0, 0.0, 8.0, 0.1, 7.9)]
    [InlineData(1.0, 0.0, 8.0, 0.0, 8.0)]
    [InlineData(2.0, 0.0, 10.0, 0.0, 8.1)]
    [InlineData(2.0, 0.0, 16.0, 1.0, 15.0)]
    [InlineData(2.0, 0.0, 16.0, 0.0, 16.0)]
    [InlineData(5.0, -5.0, 20.0, -0.1, 16.0)]
    [InlineData(5.0, 0.0, 40.0, 1.0, 39.0)]
    [InlineData(5.0, 0.0, 40.0, 0.0, 40.0)]
    [InlineData(10.0, 0.0, 50.0, 0.0, 41.0)]
    [InlineData(20.0, -20.0, 80.0, -1.0, 80.0)]
    public void AutoScale(double expectedGridLineInterval, double expectedMin, double expectedMax, params double[] dataPoints) {
        var subject = new YAxis();

        subject.AutoScale(dataPoints);

        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
    }

    [Fact]
    public void GridLines() {
        var subject = new YAxis() {            
            Min = -10,
            Max = 105,
            GridLineInterval = 20
        };

        Assert.Equal(new double[] { 0, 20, 40, 60, 80, 100 }, subject.GetGridLines());
    }
}
