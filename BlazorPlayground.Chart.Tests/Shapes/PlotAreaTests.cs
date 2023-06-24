using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class PlotAreaTests {
    [Fact]
    public void GetAttributes() {
        var subject = new PlotArea(20, 50, 80, 90);

        var result = subject.GetAttributes();

        Assert.Equal(4, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("80", Assert.Single(result, attribute => attribute.Key == "width").Value);
        Assert.Equal("90", Assert.Single(result, attribute => attribute.Key == "height").Value);
    }
}
