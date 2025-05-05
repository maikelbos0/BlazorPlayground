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

    [Theory]
    [InlineData(0, 0, false)]
    [InlineData(1, 0, true)]
    [InlineData(0, -1, true)]
    public void HasMagnitude(double x, double y, bool expectedResult) {
        var subject = new Coordinate(x, y);

        Assert.Equal(expectedResult, subject.HasMagnitude);
    }

    [Fact]
    public void Magnitude() {
        var subject = new Coordinate(-100, -75);

        Assert.Equal(125, subject.Magnitude);
    }

    [Fact]
    public void LimitMagnitude() {
        var subject = new Coordinate(-200, -150);

        var result = subject.LimitMagnitude(125);

        CoordinateAssert.Equal(new(-100, -75), result);
    }
}
