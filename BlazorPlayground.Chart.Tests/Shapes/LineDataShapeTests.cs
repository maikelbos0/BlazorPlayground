using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class LineDataShapeTests {
    [Fact]
    public void Key() {
        var subject = new LineDataShape(20, 50, 80, 90, "red", 2, 5);

        Assert.Equal("LineDataShape[2,5]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new LineDataShape(20, 50, 80, 90, "red", 2, 5);

        var result = subject.GetAttributes();

        Assert.Equal(5, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x1").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y1").Value);
        Assert.Equal("80", Assert.Single(result, attribute => attribute.Key == "x2").Value);
        Assert.Equal("90", Assert.Single(result, attribute => attribute.Key == "y2").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "stroke").Value);
    }
}
