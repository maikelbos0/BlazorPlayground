using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class XYChartTests {
    [Fact]
    public void AutoScale() {
        var subject = new XYChart() {
            DataSeries = {
                new("Foo") { -9, 0 },
                new("Bar") {-5, 19 }
            }
        };

        subject.AutoScale();

        Assert.Equal(-10, subject.YAxis.Min);
        Assert.Equal(20, subject.YAxis.Max);
        Assert.Equal(5, subject.YAxis.GridLineInterval);
    }

    [Fact]
    public void GetPlotArea() {
        var subject = new XYChart() {
            Width = 1000,
            Height = 500,
            Padding = 25,
            XAxis = {
                 Size = 50,
                 LabelClearance = 5
            },
            YAxis = {
                 Size = 75,
                 LabelClearance = 10
            }
        };

        var plotArea = subject.GetPlotArea();

        Assert.Equal(25 + 75, plotArea.X);
        Assert.Equal(25, plotArea.Y);
        Assert.Equal(1000 - 25 - 25 - 75, plotArea.Width);
        Assert.Equal(500 - 25 - 25 - 50, plotArea.Height);
    }
}
