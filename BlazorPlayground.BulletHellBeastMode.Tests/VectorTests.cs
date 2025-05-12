using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class VectorTests {
    [Fact]
    public void OperatorAdd() {
        var a = new Coordinate(-50, 125);
        var b = new Coordinate(25, -75);

        var result = a + b;

        VectorAssert.Equal(new(-25, 50), result);
    }
    
    [Fact]
    public void OperatorSubtract() {
        var a = new Coordinate(-50, 125);
        var b = new Coordinate(25, -75);

        var result = a - b;

        VectorAssert.Equal(new(-75, 200), result);
    }
    
    [Fact]
    public void OperatorMultiply() {
        var subject = new Coordinate(-50, 125);

        var result = subject * 10;

        VectorAssert.Equal(new(-500, 1250), result);
    }
    
    [Fact]
    public void OperatorDivide() {
        var subject = new Coordinate(-50, 125);

        var result = subject / 10;

        VectorAssert.Equal(new(-5, 12.5), result);
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

        VectorAssert.Equal(new(-100, -75), result);
    }

    [Theory]
    [InlineData(125, -100, -75)]
    [InlineData(375, -300, -225)]
    public void AdjustMagnitude(double newMagnitude, double expectedX, double expectedY) {
        var subject = new Coordinate(-200, -150);

        var result = subject.AdjustMagnitude(newMagnitude);

        VectorAssert.Equal(new(expectedX, expectedY), result);
    }
}
