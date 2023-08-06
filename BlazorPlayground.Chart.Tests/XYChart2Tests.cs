using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class XYChart2Tests {
    [Fact]
    public void DataPointWidth() {
        var subject = new XYChart2() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 80,
                YAxisLabelClearance = 10
            },
            Labels = { "Foo", "Bar", "Baz" }
        };

        Assert.Equal(870M / 3M, subject.DataPointWidth);
    }

    [Theory]
    [MemberData(nameof(MapDataPointToCanvas_Data))]
    public void MapDataPointToCanvas(decimal dataPoint, decimal expectedValue) {
        var subject = new XYChart2() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M
            }
        };

        Assert.Equal(expectedValue, subject.MapDataPointToCanvas(dataPoint));
    }

    public static TheoryData<decimal, decimal> MapDataPointToCanvas_Data() {
        var plotAreaY = 25;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 500M;
        var plotAreaRange = plotAreaMax - -100M;

        return new() {
            { 50M, plotAreaY + (plotAreaMax - 50M) / plotAreaRange * plotAreaHeight },
            { 200M, plotAreaY + (plotAreaMax - 200M) / plotAreaRange * plotAreaHeight },
            { 350M, plotAreaY + (plotAreaMax - 350M) / plotAreaRange * plotAreaHeight }
        };
    }

    [Theory]
    [MemberData(nameof(MapDataValueToPlotArea_Data))]
    public void MapDataValueToPlotArea(decimal dataPoint, decimal expectedValue) {
        var subject = new XYChart2() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M
            }
        };

        Assert.Equal(expectedValue, subject.MapDataValueToPlotArea(dataPoint));
    }

    public static TheoryData<decimal, decimal> MapDataValueToPlotArea_Data() {
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 500M;
        var plotAreaRange = plotAreaMax - -100M;

        return new() {
            { 50M, 50M / plotAreaRange * plotAreaHeight },
            { 200M, 200M / plotAreaRange * plotAreaHeight },
            { 350M, 350M / plotAreaRange * plotAreaHeight }
        };
    }

    [Theory]
    [MemberData(nameof(MapDataIndexToCanvas_Data))]
    public void MapDataIndexToCanvas(int index, decimal expectedValue) {
        var subject = new XYChart2() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 100,
                YAxisLabelClearance = 10
            },
            Labels = { "Foo", "Bar", "Baz" }
        };

        Assert.Equal(expectedValue, subject.MapDataIndexToCanvas(index));
    }

    public static TheoryData<int, decimal> MapDataIndexToCanvas_Data() {
        var plotAreaX = 25 + 100;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 3M;

        return new() {
            { 0, plotAreaX + 0.5M * dataPointWidth },
            { 1, plotAreaX + 1.5M * dataPointWidth },
            { 2, plotAreaX + 2.5M * dataPointWidth },
        };
    }
}
