using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class XAxisLabelShapeTests {
    [Fact]
    public void Key() {
        var subject = new XAxisLabelShape(100M, 50M, "Foo", 2);

        Assert.Equal("XAxisLabelShape[2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new XAxisLabelShape(100M, 50M, "Foo", 2);

        var result = subject.GetAttributes();

        Assert.Equal(2, result.Count());
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
    }

    [Fact]
    public void GetContent() {
        var subject = new XAxisLabelShape(100M, 50M, "Foo", 2);

        Assert.Equal("Foo", subject.GetContent());
    }
}
