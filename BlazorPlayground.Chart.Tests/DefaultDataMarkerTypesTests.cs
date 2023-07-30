using BlazorPlayground.Chart.Shapes;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DefaultDataMarkerTypesTests {
    [Fact]
    public void Round() {
        var result = DefaultDataMarkerTypes.Round(50M, 150M, 10M, "red", 1, 5);

        var shape = Assert.IsType<RoundDataMarkerShape>(result);

        Assert.Equal(50M, shape.X);
        Assert.Equal(150M, shape.Y);
        Assert.Equal(10M, shape.Size);
        Assert.Equal("red", shape.Color);
        Assert.EndsWith("[1,5]", shape.Key);
    }

    [Fact]
    public void Square() {
        var result = DefaultDataMarkerTypes.Square(50M, 150M, 10M, "red", 1, 5);

        var shape = Assert.IsType<SquareDataMarkerShape>(result);

        Assert.Equal(50M, shape.X);
        Assert.Equal(150M, shape.Y);
        Assert.Equal(10M, shape.Size);
        Assert.Equal("red", shape.Color);
        Assert.EndsWith("[1,5]", shape.Key);
    }
}
