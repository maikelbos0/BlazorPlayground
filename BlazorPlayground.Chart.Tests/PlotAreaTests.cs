using System;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class PlotAreaTests {
    [Theory]
    [MemberData(nameof(AutoScaleData))]
    public void AutoScale(decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax, params decimal[] dataPoints) {
        var subject = new PlotArea();

        subject.AutoScale(dataPoints);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<decimal, decimal, decimal, decimal[]> AutoScaleData() => new() {
        { 1M, 0M, 5M, Array.Empty<decimal>() },
        { 1M, 100M, 106M, new decimal[] { 103M } },
        { 1M, 0M, 5M, new decimal[] { 0.2M, 4.8M } },
        { 2M, -2M, 10M, new decimal[] { -0.2M, 8.2M } },
        { 0.1M, 0M, 0.8M, new decimal[] { 0.01M, 0.79M } },
        { 1M, 0M, 8M, new decimal[] { 0.1M, 7.9M } },
        { 1M, 0M, 8M, new decimal[] { 0M, 8M } },
        { 2M, 0M, 10M, new decimal[] { 0M, 8.1M } },
        { 2M, 0M, 16M, new decimal[] { 1M, 15M } },
        { 2M, 0M, 16M, new decimal[] { 0M, 16M } },
        { 5M, -5M, 20M, new decimal[] { -0.1M, 16M } },
        { 5M, 0M, 40M, new decimal[] { 1M, 39M } },
        { 5M, 0M, 40M, new decimal[] { 0M, 40M } },
        { 10M, 0M, 50M, new decimal[] { 0M, 41M } },
        { 20M, -20M, 80M, new decimal[] { -1M, 80M } },
        { 0.2M, -0.6M, 0.6M, new decimal[] { -0.5M, 0.5M } }
    };

    [Theory]
    [MemberData(nameof(GetGridLineDataPointsData))]
    public void GetGridLineDataPoints(decimal min, decimal max, decimal gridLineInterval, params decimal[] expectedDataPoints) {
        var subject = new PlotArea() {
            Min = min,
            Max = max,
            GridLineInterval = gridLineInterval
        };

        Assert.Equal(expectedDataPoints, subject.GetGridLineDataPoints());
    }

    public static TheoryData<decimal, decimal, decimal, decimal[]> GetGridLineDataPointsData() => new() {
        { 0M, 100M, 20M,  new decimal[] { 0M, 20M, 40M, 60M, 80M, 100M } },
        { -10M, 105M, 20M,  new decimal[] { 0M, 20M, 40M, 60M, 80M, 100.0M } },
        { -1M, 0.2M, 0.2M, new decimal[] { -1M, -0.8M, -0.6M, -0.4M, -0.2M, 0M, 0.2M } }
    };
}
