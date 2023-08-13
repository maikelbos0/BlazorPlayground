using BlazorPlayground.Chart.Shapes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class XYChartTests {
    [Fact]
    public void SetCanvas() {
        var stateHasChangedInvoked = false;
        var canvas = new Canvas();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true
        };

        subject.SetCanvas(canvas);

        Assert.Same(canvas, subject.Canvas);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void ResetCanvas() {
        var stateHasChangedInvoked = false;
        var canvas = new Canvas();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true,
            Canvas = canvas
        };

        subject.ResetCanvas();

        Assert.NotSame(canvas, subject.Canvas);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void SetPlotArea() {
        var stateHasChangedInvoked = false;
        var plotArea = new PlotArea();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true
        };

        subject.SetPlotArea(plotArea);

        Assert.Same(plotArea, subject.PlotArea);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void ResetPlotArea() {
        var stateHasChangedInvoked = false;
        var plotArea = new PlotArea();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true,
            PlotArea = plotArea
        };

        subject.ResetPlotArea();

        Assert.NotSame(plotArea, subject.PlotArea);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void AddLayer() {
        var stateHasChangedInvoked = false;
        var layer = new BarLayer();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true
        };

        subject.AddLayer(layer);

        Assert.Same(layer, Assert.Single(subject.Layers));
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void RemoveLayer() {
        var stateHasChangedInvoked = false;
        var layer = new BarLayer();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true,
            Layers = {
                layer
            }
        };

        subject.RemoveLayer(layer);

        Assert.Empty(subject.Layers);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void GetShapes_AutoScale() {
        var subject = new XYChart() {
            PlotArea = {
                Min = -4M,
                Max = 10M,
                GridLineInterval = 1M,
                AutoScaleSettings = {
                    IsEnabled = true,
                    ClearancePercentage = 0M
                }
            },
            Canvas = {
                Height = 800,
                Padding = 25,
                XAxisLabelHeight = 50
            },
            Labels = { "Foo", "Bar", "Baz" },
        };

        subject.Layers.Add(new BarLayer() {
            Chart = subject,
            IsStacked = false,
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { -9M, 0M }
                },
                new() {
                    Color = "red",
                    DataPoints = { -5M, 19M }
                }
            }
        });

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
                GridLineInterval = 1M,
                AutoScaleSettings = {
                    IsEnabled = false
                }
            },
            Canvas = {
                Height = 800,
                Padding = 25,
                XAxisLabelHeight = 50
            },
            Labels = { "Foo", "Bar", "Baz" }
        };

        subject.Layers.Add(new BarLayer() {
            Chart = subject,
            IsStacked = false,
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { -9M, 0M }
                },
                new() {
                    Color = "red",
                    DataPoints = { -5M, 19M }
                }
            }
        });

        _ = subject.GetShapes().ToList();

        Assert.Equal(-4M, subject.PlotArea.Min);
        Assert.Equal(10M, subject.PlotArea.Max);
        Assert.Equal(1M, subject.PlotArea.GridLineInterval);
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

        subject.Layers.Add(new BarLayer() {
            Chart = subject,
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { 5M, 10M }
                }
            }
        });

        Assert.Contains(subject.GetShapes(), shape => shape is DataBarShape);
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

        var plotAreaRange = subject.PlotArea.Max - subject.PlotArea.Min;

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.Y == subject.Canvas.PlotAreaY + (400M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight);
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.Y == subject.Canvas.PlotAreaY + (200M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight);
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.Y == subject.Canvas.PlotAreaY + (0M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight);
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

        var plotAreaRange = subject.PlotArea.Max - subject.PlotArea.Min;

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.Y == subject.Canvas.PlotAreaY + (400M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight && shape.Value == "000");
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.Y == subject.Canvas.PlotAreaY + (200M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight && shape.Value == "020");
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.Y == subject.Canvas.PlotAreaY + (0M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight && shape.Value == "040");
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
                YAxisLabelWidth = 80,
                YAxisLabelClearance = 10
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M,
                 Multiplier = 1000
            },
            Labels = { "Foo", "Bar", "Baz" },
            DataPointSpacingMode = DataPointSpacingMode.Center
        };

        var result = subject.GetXAxisLabelShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal(500 - 25 - 50 + 5, shape.Y);
        });

        var dataPointWidth = 870M / 3;

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.X == subject.Canvas.PlotAreaX + 0.5M * dataPointWidth && shape.Label == "Foo");
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.X == subject.Canvas.PlotAreaX + 1.5M * dataPointWidth && shape.Label == "Bar");
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.X == subject.Canvas.PlotAreaX + 2.5M * dataPointWidth && shape.Label == "Baz");
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
                YAxisLabelWidth = 100,
                YAxisLabelClearance = 10
            },
            PlotArea = {
                    Min = -10M,
                    Max = 40M,
                    GridLineInterval = 10M
            },
            Labels = { "Foo", "Bar", "Baz" }
        };

        subject.Layers.Add(new BarLayer() {
            Chart = subject,
            DataSeries = {
                new() {
                    Color = "red",
                    DataPoints = { 5M, null, 15M }
                }
            }
        });

        subject.Layers.Add(new BarLayer() {
            Chart = subject,
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { 11M, 8M, null }
                }
            }
        });

        var result = subject.GetDataSeriesShapes();

        Assert.Equal(4, result.Count());

        Assert.All(result, shape => Assert.IsType<DataBarShape>(shape));
    }

    [Theory]
    [MemberData(nameof(MapDataPointToCanvas_Data))]
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
    [MemberData(nameof(GetDataPointSpacingMode_Data))]
    public void GetDataPointSpacingMode(DataPointSpacingMode dataPointSpacingMode, List<LayerBase> layers, DataPointSpacingMode expectedDataPointSpacingMode) {
        var subject = new XYChart() {
            Layers = layers,
            DataPointSpacingMode = dataPointSpacingMode
        };

        Assert.Equal(expectedDataPointSpacingMode, subject.GetDataPointSpacingMode());
    }

    public static TheoryData<DataPointSpacingMode, List<LayerBase>, DataPointSpacingMode> GetDataPointSpacingMode_Data() => new() {
        { DataPointSpacingMode.Center, new List<LayerBase> { new AreaLayer() }, DataPointSpacingMode.Center },
        { DataPointSpacingMode.EdgeToEdge, new List<LayerBase> { new BarLayer() }, DataPointSpacingMode.EdgeToEdge },
        { DataPointSpacingMode.Auto, new List<LayerBase>(), DataPointSpacingMode.EdgeToEdge },
        { DataPointSpacingMode.Auto, new List<LayerBase> { new AreaLayer() }, DataPointSpacingMode.EdgeToEdge },
        { DataPointSpacingMode.Auto, new List<LayerBase> { new BarLayer() }, DataPointSpacingMode.Center },
        { DataPointSpacingMode.Auto, new List<LayerBase> { new LineLayer() }, DataPointSpacingMode.Center }
    };

    [Theory]
    [MemberData(nameof(GetDataPointWidth_Data))]
    public void GetDataPointWidth(DataPointSpacingMode dataPointSpacingMode, List<string> labels, decimal expectedWidth) {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 80,
                YAxisLabelClearance = 10
            },
            Labels = labels,
            DataPointSpacingMode = dataPointSpacingMode
        };

        Assert.Equal(expectedWidth, subject.GetDataPointWidth());
    }

    public static TheoryData<DataPointSpacingMode, List<string>, decimal> GetDataPointWidth_Data() => new() {
        { DataPointSpacingMode.EdgeToEdge, new List<string>() { "Foo", "Bar", "Baz" }, 870M / 2 },
        { DataPointSpacingMode.Center, new List<string>() { "Foo", "Bar", "Baz" }, 870M / 3 },
        { DataPointSpacingMode.EdgeToEdge, new List<string>() { "Foo", }, 870M },
        { DataPointSpacingMode.Center, new List<string>() { "Foo" }, 870M },
        { DataPointSpacingMode.EdgeToEdge, new List<string>(), 870M },
        { DataPointSpacingMode.Center, new List<string>(), 870M },
    };

    [Theory]
    [MemberData(nameof(MapDataIndexToCanvas_Data))]
    public void MapDataIndexToCanvas(DataPointSpacingMode dataPointSpacingMode, int index, decimal expectedValue) {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 80,
                YAxisLabelClearance = 10
            },
            Labels = { "Foo", "Bar", "Baz" },
            DataPointSpacingMode = dataPointSpacingMode
        };

        Assert.Equal(expectedValue, subject.MapDataIndexToCanvas(index));
    }

    public static TheoryData<DataPointSpacingMode, int, decimal> MapDataIndexToCanvas_Data() {
        var plotAreaX = 25 + 80;

        return new() {
            { DataPointSpacingMode.EdgeToEdge, 0, plotAreaX },
            { DataPointSpacingMode.EdgeToEdge, 1, plotAreaX + 1 * 870M / 2 },
            { DataPointSpacingMode.EdgeToEdge, 2, plotAreaX + 2 * 870M / 2 },
            { DataPointSpacingMode.Center, 0, plotAreaX + 0.5M * 870M / 3 },
            { DataPointSpacingMode.Center, 1, plotAreaX + 1.5M * 870M / 3 },
            { DataPointSpacingMode.Center, 2, plotAreaX + 2.5M * 870M / 3 },
        };
    }
}
