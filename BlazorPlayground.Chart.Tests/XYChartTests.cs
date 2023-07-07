using BlazorPlayground.Chart.Shapes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class XYChartTests {
    [Theory]
    [InlineData(0, "red")]
    [InlineData(1, "blue")]
    [InlineData(2, "green")]
    [InlineData(3, "red")]
    public void GetColor(int index, string expectedColor) {
        XYChart.DefaultColors = new List<string>() { "red", "blue", "green" };

        Assert.Equal(expectedColor, XYChart.GetColor(index));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1, "red", "red", "blue", "green")]
    public void GetColor_Fallback(int index, params string[] defaultColors) {
        XYChart.DefaultColors = defaultColors.ToList();

        Assert.Equal(XYChart.FallbackColor, XYChart.GetColor(index));
    }

    [Fact]
    public void GetShapes_AutoScale() {
        var subject = new XYChart() {
            PlotArea = {
                Min = -4M,
                Max = 10M,
                GridLineInterval = 2M
            },
            Canvas = {
                Height = 800,
                Padding = 25,
                XAxisLabelHeight = 50
            },
            DataSeries = {
                new("Foo", "red") { -9M, 0M },
                new("Bar", "blue") {-5M, 19M }
            },
            AutoScaleSettings = {
                IsEnabled = true
            }
        };

        _ = subject.GetShapes().ToList();

        Assert.Equal(-10M, subject.PlotArea.Min);
        Assert.Equal(20M, subject.PlotArea.Max);
        Assert.Equal(2M, subject.PlotArea.GridLineInterval);
    }

    [Fact]
    public void GetShapes_No_AutoScale() {
        var subject = new XYChart() {
            PlotArea = {
                Min = -4M,
                Max = 10M,
                GridLineInterval = 2M
            },
            Canvas = {
                Height = 800,
                Padding = 25,
                XAxisLabelHeight = 50
            },
            DataSeries = {
                new("Foo", "red") { -9M, 0M },
                new("Bar", "blue") {-5M, 19M }
            },
            AutoScaleSettings = {
                IsEnabled = false
            }
        };

        _ = subject.GetShapes().ToList();

        Assert.Equal(-4M, subject.PlotArea.Min);
        Assert.Equal(10M, subject.PlotArea.Max);
        Assert.Equal(2M, subject.PlotArea.GridLineInterval);
    }

    [Fact]
    public void GetShapes_PlotAreaShape() {
        var subject = new XYChart();

        Assert.Single(subject.GetShapes(), shape => shape is PlotAreaShape);
    }

    [Fact]
    public void GetShapes_GridLineShapes() {
        var subject = new XYChart();

        Assert.Contains(subject.GetShapes(), shape => shape is GridLineShape);
    }

    [Fact]
    public void GetShapes_YAxisLabelShapes() {
        var subject = new XYChart();

        Assert.Contains(subject.GetShapes(), shape => shape is YAxisLabelShape);
    }

    [Fact]
    public void GetShapes_DataSeriesShapes() {
        var subject = new XYChart() {
            DataSeries = {
                new("Foo", "red") { 5, 10 }
            },
            Labels = { "Foo", "Bar" }
        };

        Assert.Contains(subject.GetShapes(), shape => shape is BarDataShape);
    }

    [Fact]
    public void GetGridLineShapes() {
        var subject = new XYChart() {
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
                 Max = 500M,
                 GridLineInterval = 200M
            }
        };

        var result = subject.GetGridLineShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal(25 + 75, shape.X);
            Assert.Equal(1000 - 25 - 25 - 75, shape.Width);
        });

        Assert.Single(result, shape => shape.Y == 25 + (0M - -100M) / 600.0M * (500 - 25 - 25 - 50));
        Assert.Single(result, shape => shape.Y == 25 + (200M - -100M) / 600.0M * (500 - 25 - 25 - 50));
        Assert.Single(result, shape => shape.Y == 25 + (400M - -100M) / 600.0M * (500 - 25 - 25 - 50));
    }

    [Fact]
    public void GetYAxisLabelShapes() {
        var subject = new XYChart() {
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
                 Max = 500M,
                 GridLineInterval = 200M
            }
        };

        var result = subject.GetYAxisLabelShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal(25 + 75 - 10, shape.X);
        });

        Assert.Single(result, shape => shape.Y == 25 + (0M - -100M) / 600.0M * (500 - 25 - 25 - 50) && shape.Value == 400M);
        Assert.Single(result, shape => shape.Y == 25 + (200M - -100M) / 600.0M * (500 - 25 - 25 - 50) && shape.Value == 200M);
        Assert.Single(result, shape => shape.Y == 25 + (400M - -100M) / 600.0M * (500 - 25 - 25 - 50) && shape.Value == 0M);
    }

    [Theory]
    [MemberData(nameof(GetDataSeriesShapesData))]
    public void GetDataSeriesShapes(int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedWidth, decimal expectedHeight) {
        var subject = new XYChart() {
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
            DataSeries = {
                new("Foo", "red") { null, null, null, null, 15M }
            },
            Labels = { "Foo", "Bar", "Baz", "Quux" }
        };

        subject.DataSeries[0][index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.Single(result);

        Assert.Equal("red", shape.Color);
        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedWidth, shape.Width);
        Assert.Equal(expectedHeight, shape.Height);

        // TODO move this around
        // TODO fix x axis to include configurable width and offset for multiples
    }

    public static TheoryData<int, decimal, decimal, decimal, decimal, decimal> GetDataSeriesShapesData() => new() {
        { 0, -5M, 25M + 100M + 0.5M * 850M / 4M - 5M, 25M + 40M / 50M * 400M, 10M, 5M / 50M * 400M },
        { 1, 5M, 25M + 100M + 1.5M * 850M / 4M - 5M, 25M + (40M - 5M) / 50M * 400M, 10M, 5M / 50M * 400M },
        { 3, 35M, 25M + 100M + 3.5M * 850M / 4M - 5M, 25M + (40M - 35M) / 50M * 400M, 10M, 35M / 50M * 400M },
    };

    [Theory]
    [MemberData(nameof(MapDataPointToCanvasData))]
    public void MapDataPointToCanvas(decimal dataPoint, decimal expectedValue) {
        var subject = new XYChart() {
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
                 Min = -100,
                 Max = 500
            }
        };

        Assert.Equal(expectedValue, subject.MapDataPointToCanvas(dataPoint));
    }

    public static TheoryData<decimal, decimal> MapDataPointToCanvasData() => new() {
        { 50M, 25 + 300M },
        { 200M, 25 + 200M },
        { 350M, 25 + 100M }
    };

    [Theory]
    [MemberData(nameof(MapDataIndexToCanvasData))]
    public void MapDataIndexToCanvas(int index, decimal expectedValue) {
        var subject = new XYChart() {
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

    public static TheoryData<int, decimal> MapDataIndexToCanvasData() => new() {
        { 0, 125 + 0.5M * 850M / 3M },
        { 1, 125 + 1.5M * 850M / 3M },
        { 2, 125 + 2.5M * 850M / 3M },
    };

    [Fact]
    public void AddDataSeries_With_Color() {
        var subject = new XYChart() {
            Labels = {
                "Foo",
                "Bar",
                "Baz"
            }
        };

        var result = subject.AddDataSeries("Data", "red");

        Assert.Same(result, Assert.Single(subject.DataSeries));
        Assert.Equal("Data", result.Name);
        Assert.Equal("red", result.Color);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void AddDataSeries_Without_Color() {
        XYChart.DefaultColors = new List<string>() { "red", "blue", "green" };

        var subject = new XYChart() {
            Labels = {
                "Foo",
                "Bar",
                "Baz"
            }
        };

        var result = subject.AddDataSeries("Data");

        Assert.Same(result, Assert.Single(subject.DataSeries));
        Assert.Equal("Data", result.Name);
        Assert.Equal("red", result.Color);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void AddDataPoint() {
        var subject = new XYChart() {
            Labels = {
                "Value 1",
                "Value 2"
            },
            DataSeries = {
                new("Foo", "red") { 2.5M, 3M, 4M },
                new("Bar", "blue") { 5.5M, 6M },
                new("Baz", "green")
            }
        };

        subject.AddDataPoint("Value 3");

        Assert.Equal(3, subject.Labels.Count);
        Assert.Equal(new List<string> { "Value 1", "Value 2", "Value 3" }, subject.Labels);
        Assert.Equal(new List<decimal?> { 2.5M, 3M, 4M }, subject.DataSeries[0]);
        Assert.Equal(new List<decimal?> { 5.5M, 6M, null }, subject.DataSeries[1]);
        Assert.Equal(new List<decimal?> { null, null, null }, subject.DataSeries[2]);
    }
}
