using BlazorPlayground.Chart.Shapes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class XYChartTests {
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
            AutoScaleSettings = {
                IsEnabled = true,
                ClearancePercentage = 0M
            }
        };

        subject.DataSeriesLayers.Add(new BarDataSeriesLayer(subject) {
            DataSeries = {
                new("Foo", "red") { -9M, 0M },
                new("Bar", "blue") {-5M, 19M }
            }
        });

        _ = subject.GetShapes().ToList();

        Assert.Equal(-10M, subject.PlotArea.Min);
        Assert.Equal(20M, subject.PlotArea.Max);
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
    public void GetShapes_YAxisMultiplierShape() {
        var subject = new XYChart() {
            PlotArea = {
                Multiplier = 1000
            }
        };

        Assert.Single(subject.GetShapes(), shape => shape is YAxisMultiplierShape);
    }

    [Fact]
    public void GetShapes_YAxisMultiplierShape_Without_Multiplier() {
        var subject = new XYChart();

        Assert.DoesNotContain(subject.GetShapes(), shape => shape == null);
    }

    [Fact]
    public void GetShapes_XAxisLabelShapes() {
        var subject = new XYChart() {
            Labels = { "Foo", "Bar" }
        };

        Assert.Contains(subject.GetShapes(), shape => shape is XAxisLabelShape);
    }

    [Fact]
    public void GetShapes_DataSeriesShapes() {
        var subject = new XYChart() {
            Labels = { "Foo", "Bar" }
        };

        subject.DataSeriesLayers.Add(new BarDataSeriesLayer(subject) {
            DataSeries = {
                new("Foo", "red") { 5M, 10M }
            }
        });

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

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.Y == 25 + (400M - -100M) / 600.0M * (500 - 25 - 25 - 50));
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.Y == 25 + (200M - -100M) / 600.0M * (500 - 25 - 25 - 50));
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.Y == 25 + (0M - -100M) / 600.0M * (500 - 25 - 25 - 50));
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
                YAxisLabelClearance = 10,
                YAxisLabelFormat = "#000"
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M,
                 Multiplier = 10M
            }
        };

        var result = subject.GetYAxisLabelShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal(25 + 75 - 10, shape.X);
        });

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.Y == 25 + (400M - -100M) / 600.0M * (500 - 25 - 25 - 50) && shape.Value == "000");
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.Y == 25 + (200M - -100M) / 600.0M * (500 - 25 - 25 - 50) && shape.Value == "020");
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.Y == 25 + (0M - -100M) / 600.0M * (500 - 25 - 25 - 50) && shape.Value == "040");
    }

    [Fact]
    public void GetYAxisMultiplierShape() {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10,
                YAxisMultiplierFormat = "x 0000"
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M,
                 Multiplier = 1000
            }
        };

        var result = subject.GetYAxisMultiplierShape();

        Assert.NotNull(result);
        Assert.Equal(25, result.X);
        Assert.Equal(25 + (500 - 25 - 25 - 50) / 2, result.Y);
        Assert.Equal("x 1000", result.Multiplier);
    }

    [Fact]
    public void GetYAxisMultiplierShape_Without_Multiplier() {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10,
                YAxisMultiplierFormat = "x 0000"
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M
            }
        };

        var result = subject.GetYAxisMultiplierShape();

        Assert.Null(result);
    }

    [Fact]
    public void GetXAxisLabelShapes() {
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
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M,
                 Multiplier = 1000
            },
            Labels = { "Foo", "Bar", "Baz" }
        };

        var result = subject.GetXAxisLabelShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal(500 - 25 - 50 + 5, shape.Y);
        });

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.X == 25M + 100M + 0.5M * 850M / 3M && shape.Label == "Foo");
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.X == 25M + 100M + 1.5M * 850M / 3M && shape.Label == "Bar");
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.X == 25M + 100M + 2.5M * 850M / 3M && shape.Label == "Baz");
    }

    [Fact]
    public void GetDataSeriesShapes() {
        Assert.Fail("TODO");
    }

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
    public void AddBarLayer() {
        Assert.Fail("TODO");
    }

    [Fact]
    public void AddDataPoint() {
        var subject = new XYChart() {
            Labels = {
                "Value 1",
                "Value 2"
            }
        };

        var layer = new BarDataSeriesLayer(subject) {
            DataSeries = {
                new("Foo", "red") { 2.5M, 3M, 4M },
                new("Bar", "blue") { 5.5M, 6M },
                new("Baz", "green")
            }
        };

        subject.DataSeriesLayers.Add(layer);

        subject.AddDataPoint("Value 3");

        Assert.Equal(3, subject.Labels.Count);
        Assert.Equal(new List<string> { "Value 1", "Value 2", "Value 3" }, subject.Labels);
        Assert.Equal(new List<decimal?> { 2.5M, 3M, 4M }, layer.DataSeries[0]);
        Assert.Equal(new List<decimal?> { 5.5M, 6M, null }, layer.DataSeries[1]);
        Assert.Equal(new List<decimal?> { null, null, null }, layer.DataSeries[2]);
    }
}
