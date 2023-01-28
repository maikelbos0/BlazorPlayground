using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class YAxisTests {
    [Fact]
    public void GridLines() {
        var subject = new YAxis(20, -10, 105);

        Assert.Equal(new double[] { 0, 20, 40, 60, 80, 100 }, subject.GetGridLines());
    }
}
