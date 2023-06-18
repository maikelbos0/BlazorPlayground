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
}
