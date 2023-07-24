﻿using BlazorPlayground.Chart.Shapes;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class BarDataSeriesLayerTests {
    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Data))]
    public void GetUnstackedDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedWidth, decimal expectedHeight) {
        var subject = new BarDataSeriesLayer(
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
            ClearancePercentage = 25M,
            GapPercentage = 10M,
            IsStacked = false
        };

        subject.DataSeries[dataSeriesIndex][index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<BarDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(BarDataShape)}[{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedWidth, shape.Width);
        Assert.Equal(expectedHeight, shape.Height);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal, decimal> GetUnstackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 40M;
        var plotAreaRange = plotAreaMax - -10M;

        return new() {
            { 0, 0, -5M, plotAreaX + (0.5M - 0.25M) * dataPointWidth, plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight, 0.2M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight },
            { 1, 1, 5M, plotAreaX + (1.5M + 0.1M / 2) * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 0.2M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight },
            { 0, 3, 35M, plotAreaX + (3.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaMax - 35M) / plotAreaRange * plotAreaHeight, 0.2M * dataPointWidth, 35M / plotAreaRange * plotAreaHeight },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Data))]
    public void GetStackedDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedWidth, decimal expectedHeight) {
        var subject = new BarDataSeriesLayer(
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
            ClearancePercentage = 25M,
            GapPercentage = 10M,
            IsStacked = true
        };

        subject.DataSeries[dataSeriesIndex][index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<BarDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(BarDataShape)}[{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedWidth, shape.Width);
        Assert.Equal(expectedHeight, shape.Height);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
    }

    // TODO add first series test case?
    public static TheoryData<int, int, decimal, decimal, decimal, decimal, decimal> GetStackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (1000 - 25 - 25 - 100) / 4M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaMax = 30M;
        var plotAreaRange = plotAreaMax - -20M;

        return new() {
            { 1, 0, -5M, plotAreaX + (0.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaMax + 10M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight },
            { 1, 0, 5M, plotAreaX + (0.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaMax - 5M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight },
            { 1, 2, -5M, plotAreaX + (2.5M - 0.25M) * dataPointWidth, plotAreaY + plotAreaMax / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight },
            { 1, 2, 5M, plotAreaX + (2.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaMax - 15M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight }
        };
    }
}
