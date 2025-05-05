using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class CoordinateTests {
    [Fact]
    public void OperatorPlus() {
        var a = new Coordinate(-50, 125);
        var b = new Coordinate(25, -75);

        var result = a + b;

        CoordinateAssert.Equal(new(-25, 50), result);
    }
    
    [Fact]
    public void OperatorMinus() {
        var a = new Coordinate(-50, 125);
        var b = new Coordinate(25, -75);

        var result = a - b;

        CoordinateAssert.Equal(new(-75, 200), result);
    }

    [Fact]
    public void GetMagnitude() {
        var subject = new Coordinate(-100, -75);

        var result = subject.GetMagnitude();

        Assert.Equal(125, result);
    }

    [Fact]
    public void LimitMagnitude() {
        var subject = new Coordinate(-200, -150);

        var result = subject.LimitMagnitude(125);

        CoordinateAssert.Equal(new(-100, -75), result);
    }
}
