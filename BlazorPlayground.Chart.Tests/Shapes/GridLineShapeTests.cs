using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class GridLineShapeTests {
    [Fact]
    public void GetAttributes() {
        var subject = new GridLineShape(20, 50, 80, 10);

        var result = subject.GetAttributes();

        Assert.Equal(5, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x1").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y1").Value);
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x2").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y2").Value);
        Assert.Equal("10", Assert.Single(result, attribute => attribute.Key == "value").Value);
    }
}
