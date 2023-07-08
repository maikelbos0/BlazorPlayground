using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class BarDataShapeTests {
    [Fact]
    public void GetKey() {
        var subject = new BarDataShape(20, 50, 80, 90, "red");

        Assert.Equal("BarDataShape/20/50/80/90/red", subject.GetKey());
    }

    [Fact]
    public void GetAttributes() {
        var subject = new BarDataShape(20, 50, 80, 90, "red");

        var result = subject.GetAttributes();

        Assert.Equal(5, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("80", Assert.Single(result, attribute => attribute.Key == "width").Value);
        Assert.Equal("90", Assert.Single(result, attribute => attribute.Key == "height").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }
}
