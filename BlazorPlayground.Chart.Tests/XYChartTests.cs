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
    public void AutoScale() {
        var subject = new XYChart() {
            DataSeries = {
                new("Foo", "red") { -9, 0 },
                new("Bar", "blue") {-5, 19 }
            }
        };

        subject.AutoScale();

        Assert.Equal(-10, subject.PlotArea.Min);
        Assert.Equal(20, subject.PlotArea.Max);
        Assert.Equal(5, subject.PlotArea.GridLineInterval);
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
                 Min = -100,
                 Max = 500,
                 GridLineInterval = 200
            }
        };

        var gridLines = subject.GetGridLineShapes();

        Assert.Equal(3, gridLines.Count());

        Assert.All(gridLines, gridLine => {
            Assert.Equal(25 + 75, gridLine.X);
            Assert.Equal(1000 - 25 - 25 - 75, gridLine.Width);
        });

        Assert.Single(gridLines, gridLine => gridLine.Y == 25 + (0 - -100) / 600.0 * (500 - 25 - 25 - 50));
        Assert.Single(gridLines, gridLine => gridLine.Y == 25 + (200 - -100) / 600.0 * (500 - 25 - 25 - 50));
        Assert.Single(gridLines, gridLine => gridLine.Y == 25 + (400 - 100) / 600.0 * (500 - 25 - 25 - 50));
    }

    [Fact]
    public void MapToPlotArea() {
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

        Assert.Equal(100, subject.MapToPlotArea(50));
    }

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
                new("Foo", "red") { 2.5, 3, 4 },
                new("Bar", "blue") { 5.5, 6 },
                new("Baz", "green")
            }
        };

        subject.AddDataPoint("Value 3");

        Assert.Equal(3, subject.Labels.Count);
        Assert.Equal(new List<string> { "Value 1", "Value 2", "Value 3" }, subject.Labels);
        Assert.Equal(new List<double?> { 2.5, 3, 4 }, subject.DataSeries[0]);
        Assert.Equal(new List<double?> { 5.5, 6, null }, subject.DataSeries[1]);
        Assert.Equal(new List<double?> { null, null, null }, subject.DataSeries[2]);
    }
}
