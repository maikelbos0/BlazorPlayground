using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class GridLineTests {
    [Fact]
    public void GetAttributes() {
        var subject = new GridLine(20, 50, 80);

        var result = subject.GetAttributes();

        Assert.Equal(4, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x1").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y1").Value);
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x2").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y2").Value);
    }
}
