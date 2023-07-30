using BlazorPlayground.Chart.Shapes;
using System;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class LineLayerTests {
    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Data))]
    public void GetUnstackedDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedSize) {
        var subject = new LineLayer(
            new XYChart() {
                Canvas = {
                    Width = 1000,
                    Height = 500,
                    Padding = 25,
                    XAxisLabelHeight = 50,
                    XAxisLabelClearance = 5,
                    YAxisLabelWidth = 100,
                    YAxisLabelClearance = 10
                },
                PlotArea = {
                     Min = -10M,
                     Max = 40M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux" }
            }
        ) {
            DataSeries = {
                new("Foo", "blue") { null, null, null, null, 15M },
                new("Bar", "red") { null, null, null, null, 15M }
            },
            IsStacked = false,
            DataMarker = (decimal x, decimal y, decimal size, string color, int dataSeriesIndex, int dataPointIndex) => new RoundDataMarkerShape(x, y, size, color, dataSeriesIndex, dataPointIndex)
        };

        subject.DataSeries[dataSeriesIndex][index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<RoundDataMarkerShape>(Assert.Single(result, shape => shape.Key == $"{nameof(RoundDataMarkerShape)}[{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedSize, shape.Size);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal> GetUnstackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 40M;
        var plotAreaRange = plotAreaMax - -10M;

        return new() {
            { 0, 0, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, 10M },
            { 1, 1, 5M, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 10M },
            { 0, 3, 35M, plotAreaX + 3.5M * dataPointWidth, plotAreaY + (plotAreaMax - 35M) / plotAreaRange * plotAreaHeight, 10M },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Data))]
    public void GetStackedDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedSize) {
        var subject = new LineLayer(
            new XYChart() {
                Canvas = {
                    Width = 1000,
                    Height = 500,
                    Padding = 25,
                    XAxisLabelHeight = 50,
                    XAxisLabelClearance = 5,
                    YAxisLabelWidth = 100,
                    YAxisLabelClearance = 10
                },
                PlotArea = {
                     Min = -20M,
                     Max = 30M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux" }
            }
        ) {
            DataSeries = {
                new("Foo", "blue") { -10M, -10M, 10M, 10M, 15M },
                new("Bar", "red") { null, null, null, null, 15M }
            },
            IsStacked = true,
            DataMarker = (decimal x, decimal y, decimal size, string color, int dataSeriesIndex, int dataPointIndex) => new RoundDataMarkerShape(x, y, size, color, dataSeriesIndex, dataPointIndex)
        };

        subject.DataSeries[dataSeriesIndex][index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<RoundDataMarkerShape>(Assert.Single(result, shape => shape.Key == $"{nameof(RoundDataMarkerShape)}[{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedSize, shape.Size);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal> GetStackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 30M;
        var plotAreaRange = plotAreaMax - -20M;

        return new() {
            { 0, 0, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, 10M },
            { 0, 2, 5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 10M },
            { 1, 0, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight, 10M },
            { 1, 0, 5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 10M },
            { 1, 2, -5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, 10M },
            { 1, 2, 5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight, 10M }
        };
    }

    [Theory]
    [MemberData(nameof(DataMarker_Data))]
    public void DataMarker(DataMarkerDelegate dataMarker, Type expectedType) {
        var subject = new LineLayer(
           new XYChart() {
               Labels = { "Foo" }
           }
        ) {
            DataSeries = {
                new("Bar", "red") { 15M }
            },
            DataMarker = dataMarker
        };

        Assert.IsType(expectedType, Assert.Single(subject.GetDataSeriesShapes()));
    }

    public static TheoryData<DataMarkerDelegate, Type> DataMarker_Data() => new() {
        { DefaultDataMarkers.Round, typeof(RoundDataMarkerShape) },
        { DefaultDataMarkers.Square, typeof(SquareDataMarkerShape) },
    };
}
