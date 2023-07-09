using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests.Shapes;

public class YAxisLabelShapeTests {
    [Fact]
    public void GetKey() {
        var subject = new YAxisLabelShape(100M, 50M, "150");

        Assert.Equal("YAxisLabelShape/100/50/150", subject.GetKey());
    }

    [Fact]
    public void GetAttributes() {
        var subject = new YAxisLabelShape(100M, 50M, "150");

        var result = subject.GetAttributes();

        Assert.Equal(2, result.Count());
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
    }

    [Fact]
    public void GetContent() {
        var subject = new YAxisLabelShape(100M, 50M, "150");

        Assert.Equal("150", subject.GetContent());
    }
}
