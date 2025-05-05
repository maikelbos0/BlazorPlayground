using BlazorPlayground.BulletHellBeastMode;
using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class CoordinateTests {
    [Fact]
    public void OperatorPlus() {
        var a = new Coordinate(-50, 125);
        var b = new Coordinate(25, -75);

        var result = a + b;

        CoordinateAssert.Equal(new(-25, 50), result);
    }
}
