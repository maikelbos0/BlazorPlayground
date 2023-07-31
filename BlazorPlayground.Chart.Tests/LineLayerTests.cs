using BlazorPlayground.Chart.Shapes;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class LineLayerTests {
    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Markers_Data))]
    public void GetUnstackedDataSeriesShapes_Markers(int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedSize) {
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
                new("Foo", "blue") { -10M, -10M, 10M, 10M, 15M },
                new("Bar", "red") { null, null, null, null, 15M }
            },
            IsStacked = false,
            ShowDataMarkers = true,
            DataMarkerSize = 20M,
            DataMarkerType = DefaultDataMarkerTypes.Round
        };

        subject.DataSeries[1][index] = dataPoint;

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
            ShowDataMarkers = true,
            DataMarkerSize = 20M,
            DataMarkerType = DefaultDataMarkerTypes.Round
        };

        subject.DataSeries[dataSeriesIndex][index] = dataPoint;

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
            { 1, 0, 5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 20M },
            { 1, 2, -5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, 20M },
            { 1, 2, 5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight, 20M }
        };
    }

    [Fact]
    public void GetStackedDataSeriesShapes_HideDataMarkers() {
        var subject = new LineLayer(
           new XYChart() {
               Labels = { "Foo" }
           }
        ) {
            DataSeries = {
                new("Bar", "red") { 15M }
            },
            ShowDataMarkers = false,
            DataMarkerType = DefaultDataMarkerTypes.Round
        };

        var result = subject.GetDataSeriesShapes();

        Assert.DoesNotContain(result, shape => shape is RoundDataMarkerShape);
    }

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Lines_Data))]
    public void GetUnstackedDataSeriesShapes_Lines(int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, decimal expectedX1, decimal expectedY1, decimal expectedX2, decimal expectedY2) {
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
                new("Foo", "blue") { -10M, -10M, 10M, 10M, 15M },
                new("Bar", "red") { null, null, null, null, 15M }
            },
            IsStacked = false,
        };

        subject.DataSeries[1][startIndex] = startDataPoint;
        subject.DataSeries[1][endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<LineDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(LineDataShape)}[1,{startIndex}]"));

        Assert.Equal(expectedX1, shape.X1);
        Assert.Equal(expectedY1, shape.Y1);
        Assert.Equal(expectedX2, shape.X2);
        Assert.Equal(expectedY2, shape.Y2);
        Assert.Equal("red", shape.Color);
    }

    public static TheoryData<int, decimal, int, decimal, decimal, decimal, decimal, decimal> GetUnstackedDataSeriesShapes_Lines_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 40M;
        var plotAreaRange = plotAreaMax - -10M;

        return new() {
            { 0, 5M, 1, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight },
            { 1, -5M, 2, 5M, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight },
            { 1, 10M, 3, 35M, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax - 10M) / plotAreaRange * plotAreaHeight, plotAreaX + 3.5M * dataPointWidth, plotAreaY + (plotAreaMax - 35M) / plotAreaRange * plotAreaHeight },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Lines_Data))]
    public void GetStackedDataSeriesShapes_Lines(int dataSeriesIndex, int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, decimal expectedX1, decimal expectedY1, decimal expectedX2, decimal expectedY2) {
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
            ShowDataMarkers = true,
            DataMarkerSize = 20M,
            DataMarkerType = DefaultDataMarkerTypes.Round
        };

        subject.DataSeries[dataSeriesIndex][startIndex] = startDataPoint;
        subject.DataSeries[dataSeriesIndex][endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<LineDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(LineDataShape)}[{dataSeriesIndex},{startIndex}]"));

        Assert.Equal(expectedX1, shape.X1);
        Assert.Equal(expectedY1, shape.Y1);
        Assert.Equal(expectedX2, shape.X2);
        Assert.Equal(expectedY2, shape.Y2);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
    }

    public static TheoryData<int, int, decimal, int, decimal, decimal, decimal, decimal, decimal> GetStackedDataSeriesShapes_Lines_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 30M;
        var plotAreaRange = plotAreaMax - -20M;

        return new() {
            { 0, 0, 5M, 1, 5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight },
            
            // TODO switch test cases
            // { 1, 0, 5M, 1, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight  },
            // { 1, 0, -5M, 1, 5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight  },
            // { 1, 2, 5M, 3, -5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight, plotAreaX + 3.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight },
            // { 1, 2, -5M, 3, 5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, plotAreaX + 3.5M * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight  },

            { 1, 0, 5M, 1, -5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight  },
            { 1, 0, -5M, 1, 5M, plotAreaX + 0.5M * dataPointWidth, plotAreaY + (plotAreaMax + 15M) / plotAreaRange * plotAreaHeight, plotAreaX + 1.5M * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight  },
            { 1, 2, 5M, 3, -5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight, plotAreaX + 3.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight },
            { 1, 2, -5M, 3, 5M, plotAreaX + 2.5M * dataPointWidth, plotAreaY + (plotAreaMax + 5M) / plotAreaRange * plotAreaHeight, plotAreaX + 3.5M * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight  },
        };
    }
}
