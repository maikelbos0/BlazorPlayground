using System;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class PlotAreaTests {
    [Theory]
    [MemberData(nameof(AutoScaleData))]
    public void AutoScale(decimal[] dataPoints, int requestedGridLineCount, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var settings = new AutoScaleSettings() { IsEnabled = true, IncludeZero = false, ClearancePercentage = 0M };
        var subject = new PlotArea();

        subject.AutoScale(settings, dataPoints, requestedGridLineCount);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<decimal[], int, decimal, decimal, decimal> AutoScaleData() => new() {
        { new[] { 1M, 49M }, 6, 10M, 0M, 50M }, // 6
        { new[] { 1M, 49M }, 11, 5M, 0M, 50M }, // 11
        { new[] { 50M, 100M }, 11, 5M, 50M, 100M }, // 11

        { new[] { 0.1M, 4.9M }, 6, 1M, 0M, 5M }, // 6
        { new[] { 0.1M, 4.9M }, 11, 0.5M, 0M, 5M }, // 11
        { new[] { 5M, 10M }, 11, 0.5M, 5M, 10M }, // 11

        { new[] { 0.001M, 0.049M }, 6, 0.01M, 0M, 0.05M }, // 6
        { new[] { 0.001M, 0.049M }, 11, 0.005M, 0M, 0.05M }, // 11
        { new[] { 0.05M, 0.1M }, 11, 0.005M, 0.05M, 0.1M }, // 11
    };

    [Theory]
    [MemberData(nameof(AutoScale_No_DataPointsData))]
    public void AutoScale_No_DataPoints(int requestedGridLineCount, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var settings = new AutoScaleSettings() { IsEnabled = true, IncludeZero = false, ClearancePercentage = 0M };
        var subject = new PlotArea();

        subject.AutoScale(settings, Array.Empty<decimal>(), requestedGridLineCount);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<int, decimal, decimal, decimal> AutoScale_No_DataPointsData() => new() {
        { 1, 5M, 0M, 5M }, // 2
        { 2, 2M, 0M, 6M }, // 4
        { 3, 2M, 0M, 6M }, // 4
        { 4, 1M, 0M, 5M }, // 6
        { 7, 1M, 0M, 5M }, // 6
        { 8, 0.5M, 0M, 5M }, // 11
        { 17, 0.5M, 0M, 5M }, // 11
        { 18, 0.2M, 0M, 5M }, // 21
        { 25, 0.2M, 0M, 5M }, // 21
    };

    [Theory]
    [MemberData(nameof(AutoScale_IncludeZeroData))]
    public void AutoScale_IncludeZero(decimal[] dataPoints, int requestedGridLineCount, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var settings = new AutoScaleSettings() { IsEnabled = true, IncludeZero = true, ClearancePercentage = 0M };
        var subject = new PlotArea();

        subject.AutoScale(settings, dataPoints, requestedGridLineCount);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<decimal[], int, decimal, decimal, decimal> AutoScale_IncludeZeroData() => new() {
        { new[] { 20M, 22M }, 5, 5M, 0M, 25M },
        { new[] { -20M, -22M }, 5, 5M, -25M, 0M },
        { new[] { 22M, -22M }, 5, 10M, -30M, 30M },
    };

    [Theory]
    [MemberData(nameof(AutoScale_ClearancePercentageData))]
    public void AutoScale_ClearancePercentage(decimal[] dataPoints, int requestedGridLineCount, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var settings = new AutoScaleSettings() { IsEnabled = true, IncludeZero = false, ClearancePercentage = 5M };
        var subject = new PlotArea();

        subject.AutoScale(settings, dataPoints, requestedGridLineCount);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<decimal[], int, decimal, decimal, decimal> AutoScale_ClearancePercentageData() => new() {
        { new[] { 0M, 20M }, 5, 5M, 0M, 25M },
        { new[] { 0M, -20M }, 5, 5M, -25M, 0M },
        { new[] { 20M, -20M }, 5, 10M, -30M, 30M },
    };

    [Fact]
    public void AutoScale_Disabled() {
        var settings = new AutoScaleSettings() { IsEnabled = false, IncludeZero = false, ClearancePercentage = 0M };
        var subject = new PlotArea();

        subject.AutoScale(settings, new[] { 0.006M, 0.044M }, 11);

        Assert.Equal(PlotArea.DefaultMin, subject.Min);
        Assert.Equal(PlotArea.DefaultMax, subject.Max);
        Assert.Equal(PlotArea.DefaultGridLineInterval, subject.GridLineInterval);
    }

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
