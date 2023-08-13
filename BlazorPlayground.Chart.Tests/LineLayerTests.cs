using BlazorPlayground.Chart.Shapes;
using System;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class LineLayerTests {
    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Markers_Data))]
    public void GetUnstackedDataSeriesShapes_Markers(int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedSize) {
        var subject = new LineLayer() {
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
                     Min = -10M,
                     Max = 40M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux" },
                DataPointSpacingMode = DataPointSpacingMode.Center
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
            ShowDataMarkers = true,
            DataMarkerSize = 20M,
            DataMarkerType = DefaultDataMarkerTypes.Round
        };

        subject.DataSeries[1].DataPoints[index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<RoundDataMarkerShape>(Assert.Single(result, shape => shape.Key == $"{nameof(RoundDataMarkerShape)}[1,{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedSize, shape.Size);
        Assert.Equal("red", shape.Color);
    }

    public static TheoryData<int, decimal, decimal, decimal, decimal> GetUnstackedDataSeriesShapes_Markers_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 40M;
        var plotAreaRange = plotAreaMax - -10M;

        return new() {
            { 0, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, 20M },
            { 1, 5M, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 20M },
            { 3, 35M, plotAreaX + 3.5M * dataPointWidth, plotAreaY + (plotAreaMax - 35M) / plotAreaRange * plotAreaHeight, 20M },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Markers_Data))]
    public void GetStackedDataSeriesShapes_Markers(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedSize) {
        var subject = new LineLayer() {
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
                     Min = -20M,
                     Max = 30M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux" },
                DataPointSpacingMode = DataPointSpacingMode.Center
            },
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { -10M, -10M, 10M, 10M, 15M }
                },
                new() {
                    Color = "red",
                    DataPoints = { null, null, null, null, 15M }
                }
            },
            IsStacked = true,
            ShowDataMarkers = true,
            DataMarkerSize = 20M,
            DataMarkerType = DefaultDataMarkerTypes.Round
        };

        subject.DataSeries[dataSeriesIndex].DataPoints[index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<RoundDataMarkerShape>(Assert.Single(result, shape => shape.Key == $"{nameof(RoundDataMarkerShape)}[{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedSize, shape.Size);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal> GetStackedDataSeriesShapes_Markers_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 30M;
        var plotAreaRange = plotAreaMax - -20M;

        return new() {
            { 0, 0, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, 20M },
            { 0, 2, 5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 20M },
            { 1, 0, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight, 20M },
            { 1, 0, 5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, 20M },
            { 1, 2, -5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 20M },
            { 1, 2, 5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight, 20M }
        };
    }

    [Fact]
    public void GetStackedDataSeriesShapes_HideDataMarkers() {
        var subject = new LineLayer() {
            Chart = new() {
                Labels = { "Foo", "Bar" }
            },
            DataSeries = {
                new() {
                    Color = "red",
                    DataPoints = { 15M }
                }
            },
            ShowDataMarkers = false,
            DataMarkerType = DefaultDataMarkerTypes.Round
        };

        var result = subject.GetDataSeriesShapes();

        Assert.DoesNotContain(result, shape => shape is RoundDataMarkerShape);
    }

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Lines_Data))]
    public void GetUnstackedDataSeriesShapes_Lines(int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new LineLayer() {
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
                     Min = -10M,
                     Max = 40M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux" },
                DataPointSpacingMode = DataPointSpacingMode.Center
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
            ShowDataLines = true,
            LineGapMode = LineGapMode.Join
        };

        subject.DataSeries[1].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[1].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<DataLineShape>(Assert.Single(result, shape => shape.Key == $"{nameof(DataLineShape)}[1]"));

        Assert.Equal(expectedPath, shape.Path);
        Assert.Equal("red", shape.Color);
    }

    public static TheoryData<int, decimal, int, decimal, string> GetUnstackedDataSeriesShapes_Lines_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 40M;
        var plotAreaRange = plotAreaMax - -10M;

        return new() {
            { 0, 5M, 1, -5M, FormattableString.Invariant($"M {plotAreaX + 0.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 1.5M * dataPointWidth} {plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight}") },
            { 1, -5M, 2, 5M, FormattableString.Invariant($"M {plotAreaX + 1.5M * dataPointWidth} {plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 2.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight}") },
            { 1, 10M, 3, 35M, FormattableString.Invariant($"M {plotAreaX + 1.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 10M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 35M) / plotAreaRange * plotAreaHeight}") },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Lines_Data))]
    public void GetStackedDataSeriesShapes_Lines(int dataSeriesIndex, int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new LineLayer() {
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
                     Min = -20M,
                     Max = 30M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz", "Quux" },
                DataPointSpacingMode = DataPointSpacingMode.Center
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
            ShowDataLines = true,
            LineGapMode = LineGapMode.Join
        };

        subject.DataSeries[dataSeriesIndex].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[dataSeriesIndex].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<DataLineShape>(Assert.Single(result, shape => shape.Key == $"{nameof(DataLineShape)}[{dataSeriesIndex}]"));

        Assert.Equal(expectedPath, shape.Path);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
    }

    public static TheoryData<int, int, decimal, int, decimal, string> GetStackedDataSeriesShapes_Lines_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 30M;
        var plotAreaRange = plotAreaMax - -20M;

        return new() {
            { 0, 0, 5M, 1, 5M, FormattableString.Invariant($"M {plotAreaX + 0.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 1.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L 656.25 185.0 L 868.75 185.0") },
            { 1, 0, 5M, 1, -5M, FormattableString.Invariant($"M {plotAreaX + 0.5M * dataPointWidth} {plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 1.5M * dataPointWidth} {plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight}") },
            { 1, 0, -5M, 1, 5M, FormattableString.Invariant($"M {plotAreaX + 0.5M * dataPointWidth} {plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 1.5M * dataPointWidth} {plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight}") },
            { 1, 2, 5M, 3, -5M, FormattableString.Invariant($"M {plotAreaX + 2.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight}") },
            { 1, 2, -5M, 3, 5M, FormattableString.Invariant($"M {plotAreaX + 2.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight} L {plotAreaX + 3.5M * dataPointWidth} {plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight}") },
        };
    }

    [Fact]
    public void GetStackedDataSeriesShapes_HideDataLines() {
        var subject = new LineLayer() {
           Chart = new() {
               Labels = { "Foo", "Bar" }
           },
            DataSeries = {
                new() {
                    Color = "red",
                    DataPoints = { 15M, 15M }
                }
            },
            ShowDataLines = false
        };

        var result = subject.GetDataSeriesShapes();

        Assert.DoesNotContain(result, shape => shape is DataLineShape);
    }

    [Theory]
    [InlineData(LineGapMode.Skip, "M 210.0 325.00 L 380.0 325.00 M 720.0 325.00 L 890.0 325.00")]
    [InlineData(LineGapMode.Join, "M 210.0 325.00 L 380.0 325.00 L 720.0 325.00 L 890.0 325.00")]
    public void GetDataSeriesShapes_LineGapMode(LineGapMode lineGapMode, string expectedPath) {
        var subject = new LineLayer() {
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
                DataPointSpacingMode = DataPointSpacingMode.Center
            },
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { 10M, 10M, null, 10M, 10M }
                }
            },
            IsStacked = true,
            ShowDataLines = true,
            LineGapMode = lineGapMode
        };

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<DataLineShape>(Assert.Single(result, shape => shape.Key == $"{nameof(DataLineShape)}[0]"));

        Assert.Equal(expectedPath, shape.Path);
    }
}
