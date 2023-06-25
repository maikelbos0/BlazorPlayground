using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class CanvasTests {
    [Fact]
    public void PlotAreaX() {
        var subject = new Canvas() {
            Width = 1000,
            Height = 500,
            Padding = 25,
            XAxisLabelHeight = 50,
            XAxisLabelClearance = 5,
            YAxisLabelWidth = 75,
            YAxisLabelClearance = 10
        };

        Assert.Equal(25 + 75, subject.PlotAreaX);
    }

    [Fact]
    public void PlotAreaY() {
        var subject = new Canvas() {
            Width = 1000,
            Height = 500,
            Padding = 25,
            XAxisLabelHeight = 50,
            XAxisLabelClearance = 5,
            YAxisLabelWidth = 75,
            YAxisLabelClearance = 10
        };

        Assert.Equal(25, subject.PlotAreaY);
    }

    [Fact]
    public void PlotAreaWidth() {
        var subject = new Canvas() {
            Width = 1000,
            Height = 500,
            Padding = 25,
            XAxisLabelHeight = 50,
            XAxisLabelClearance = 5,
            YAxisLabelWidth = 75,
            YAxisLabelClearance = 10
        };

        Assert.Equal(1000 - 25 - 25 - 75, subject.PlotAreaWidth);
    }

    [Fact]
    public void PlotAreaHeight() {
        var subject = new Canvas() {
            Width = 1000,
            Height = 500,
            Padding = 25,
            XAxisLabelHeight = 50,
            XAxisLabelClearance = 5,
            YAxisLabelWidth = 75,
            YAxisLabelClearance = 10
        };

        Assert.Equal(500 - 25 - 25 - 50, subject.PlotAreaHeight);
    }

    [Fact]
    public void GetPlotAreaShape() {
        var subject = new Canvas() {
            Width = 1000,
            Height = 500,
            Padding = 25,
            XAxisLabelHeight = 50,
            XAxisLabelClearance = 5,
            YAxisLabelWidth = 75,
            YAxisLabelClearance = 10
        };

        var result = subject.GetPlotAreaShape();

        Assert.Equal(25 + 75, result.X);
        Assert.Equal(25, result.Y);
        Assert.Equal(1000 - 25 - 25 - 75, result.Width);
        Assert.Equal(500 - 25 - 25 - 50, result.Height);
    }
}
