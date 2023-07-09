using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class GridLineShapeTests {
    [Fact]
    public void Key() {
        var subject = new GridLineShape(20M, 50M, 80, 10M, 2);

        Assert.Equal("GridLineShape[2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new GridLineShape(20M, 50M, 80, 10M, 2);

        var result = subject.GetAttributes();

        Assert.Equal(5, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x1").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y1").Value);
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x2").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y2").Value);
        Assert.Equal("10", Assert.Single(result, attribute => attribute.Key == "value").Value);
    }
}
