using BlazorPlayground.Chart.Shapes;
using System;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class AreaLayerTests {
    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Data))]
    public void GetUnstackedDataSeriesShapes(int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new AreaLayer() {
            Chart = new() {
                Canvas = {
                    Width = 1000,
                    Height = 500,
                    Padding = 25,
                    XAxisLabelHeight = 50,
                    XAxisLabelClearance = 5,
                    YAxisLabelWidth = 80,
                    YAxisLabelClearance = 10
                },
                PlotArea = {
                     Min = -10M,
                     Max = 40M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux" },
                DataPointSpacingMode = DataPointSpacingMode.EdgeToEdge
            },
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { -10M, -10M, 10M, 10M, 15M },
                },
                new() {
                    Color = "red",
                    DataPoints = { null, null, null, null, 15M }
                }
            },
            IsStacked = false,
            LineGapMode = LineGapMode.Join
        };

        subject.DataSeries[1].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[1].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<DataAreaShape>(Assert.Single(result, shape => shape.Key == $"{nameof(DataAreaShape)}[1]"));

        Assert.Equal(expectedPath, shape.Path);
        Assert.Equal("red", shape.Color);
    }

    public static TheoryData<int, decimal, int, decimal, string> GetUnstackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 80;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 80) / 3M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 40M;
        var plotAreaRange = plotAreaMax - -10M;

        return new() {
            { 0, 5M, 1, -5M, FormattableString.Invariant($"M {plotAreaX} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} L {plotAreaX} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} Z") },
            { 1, -5M, 2, 5M, FormattableString.Invariant($"M {plotAreaX + dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 2M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} Z") },
            { 1, 10M, 3, 35M, FormattableString.Invariant($"M {plotAreaX + dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaMax - 10M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3M * dataPointWidth} {plotAreaY + (plotAreaMax - 35M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3M * dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} Z") },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Data))]
    public void GetStackedDataSeriesShapes(int dataSeriesIndex, int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new AreaLayer() {
            Chart = new() {
                Canvas = {
                    Width = 1000,
                    Height = 500,
                    Padding = 25,
                    XAxisLabelHeight = 50,
                    XAxisLabelClearance = 5,
                    YAxisLabelWidth = 80,
                    YAxisLabelClearance = 10
                },
                PlotArea = {
                     Min = -20M,
                     Max = 30M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux" },
                DataPointSpacingMode = DataPointSpacingMode.EdgeToEdge
            },
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { -10M, -10M, 10M, 10M, 15M },
                },
                new() {
                    Color = "red",
                    DataPoints = { null, null, null, null, 15M }
                }
            },
            IsStacked = true,
            LineGapMode = LineGapMode.Join
        };

        subject.DataSeries[dataSeriesIndex].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[dataSeriesIndex].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<DataAreaShape>(Assert.Single(result, shape => shape.Key == $"{nameof(DataAreaShape)}[{dataSeriesIndex}]"));

        Assert.Equal(expectedPath, shape.Path);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
    }

    public static TheoryData<int, int, decimal, int, decimal, string> GetStackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 80;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 80) / 3M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 30M;
        var plotAreaRange = plotAreaMax - -20M;

        return new() {
            { 0, 0, 5M, 1, 5M, FormattableString.Invariant($"M {plotAreaX} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} L {plotAreaX} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L 685 185.0 L 975 185.0 L 975 265.0 Z") },
            { 1, 0, 5M, 1, -5M, FormattableString.Invariant($"M {plotAreaX} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} L {plotAreaX} {plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} Z") },
            { 1, 0, -5M, 1, 5M, FormattableString.Invariant($"M {plotAreaX} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} L {plotAreaX} {plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} Z") },
            { 1, 2, 5M, 3, -5M, FormattableString.Invariant($"M {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} L {plotAreaX + 2M * dataPointWidth} {plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3M * dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} Z") },
            { 1, 2, -5M, 3, 5M, FormattableString.Invariant($"M {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} L {plotAreaX + 2M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3M * dataPointWidth} {plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3M * dataPointWidth} {plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight} Z") },
        };
    }

    [Theory]
    [InlineData(LineGapMode.Skip, "M 125.0 425 L 125.0 325.00 L 337.5 325.00 L 337.5 425 Z M 762.5 425 L 762.5 325.00 L 975.0 325.00 L 975.0 425 Z")]
    [InlineData(LineGapMode.Join, "M 125.0 425 L 125.0 325.00 L 337.5 325.00 L 762.5 325.00 L 975.0 325.00 L 975.0 425 Z")]
    public void GetDataSeriesShapes_LineGapMode(LineGapMode lineGapMode, string expectedPath) {
        var subject = new AreaLayer() {
            Chart = new() {
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
                     Min = 00M,
                     Max = 40M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux", "Quuux" },
                DataPointSpacingMode = DataPointSpacingMode.EdgeToEdge
            },
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { 10M, 10M, null, 10M, 10M }
                }
            },
            IsStacked = true,
            LineGapMode = lineGapMode
        };

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<DataAreaShape>(Assert.Single(result, shape => shape.Key == $"{nameof(DataAreaShape)}[0]"));

        Assert.Equal(expectedPath, shape.Path);
    }
}
