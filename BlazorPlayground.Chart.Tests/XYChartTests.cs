﻿using BlazorPlayground.Chart.Shapes;
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
            AutoScale = true
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
            AutoScale = false
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
            }
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

    [Fact]
    public void GetDataSeriesShapes() {
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
                 Min = -10M,
                 Max = 40M,
                 GridLineInterval = 10M
            },
            DataSeries = {
                new("Foo", "red") { -5M, 5M, null, 35M }
            }
        };

        var result = subject.GetDataSeriesShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal("red", shape.Color);
        });

        // TODO move this around, add x axis tests, add min/max tests

        Assert.Single(result, shape => shape.Y == 25M + 40M / 50M * 400M && shape.Height == 5M / 50M * 400M);
        Assert.Single(result, shape => shape.Y == 25M + (40M - 5M) / 50M * 400M && shape.Height == 5M / 50M * 400M);
        Assert.Single(result, shape => shape.Y == 25M + (40M - 35M) / 50M * 400M && shape.Height == 35M / 50M * 400M);
    }

    [Theory]
    [MemberData(nameof(MapToPlotAreaData))]
    public void MapToPlotArea(decimal dataPoint, decimal expectedValue) {
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

        Assert.Equal(expectedValue, subject.MapToPlotArea(dataPoint));
    }

    public static TheoryData<decimal, decimal> MapToPlotAreaData() => new() {
        { 50M, 300M },
        { 200M, 200M },
        { 350M, 100M }
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
