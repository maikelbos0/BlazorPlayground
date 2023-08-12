using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class DataAreaShapeTests {
    [Fact]
    public void Key() {
        var subject = new DataAreaShape(new[] { "M 20 50", "L 80 90", "L 20 90 Z" }, "red", 2);

        Assert.Equal("DataAreaShape[2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new DataAreaShape(new[] { "M 20 50", "L 80 90", "L 20 90 Z" }, "red", 2);

        var result = subject.GetAttributes();

        Assert.Equal(2, result.Count());
        Assert.Equal("M 20 50 L 80 90 L 20 90 Z", Assert.Single(result, attribute => attribute.Key == "d").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }
}
