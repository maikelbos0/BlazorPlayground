using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class DataLineShapeTests {
    [Fact]
    public void Key() {
        var subject = new DataLineShape(20, 50, 80, 90, 5M, "red", 2, 5);

        Assert.Equal("DataLineShape[2,5]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new DataLineShape(20, 50, 80, 90, 5M, "red", 2, 5);

        var result = subject.GetAttributes();

        Assert.Equal(6, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x1").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y1").Value);
        Assert.Equal("80", Assert.Single(result, attribute => attribute.Key == "x2").Value);
        Assert.Equal("90", Assert.Single(result, attribute => attribute.Key == "y2").Value);
        Assert.Equal("5", Assert.Single(result, attribute => attribute.Key == "stroke-width").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "stroke").Value);
    }
}
