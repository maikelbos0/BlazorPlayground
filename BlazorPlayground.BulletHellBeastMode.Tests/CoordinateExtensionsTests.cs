using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class CoordinateExtensionsTests {
    [Theory]
    [InlineData(100, 200, 25, 50, 1, 125, 250)]
    [InlineData(100, 200, -25, -50, 1, 75, 150)]
    [InlineData(100, 200, 50, 100, 0.5, 125, 250)]
    [InlineData(100, 200, 50, 100, 2.5, 225, 450)]
    public void Move(double coordinateX, double coordinateY, double velocityX, double velocityY, double elapsedSeconds, double expectedX, double expectedY) {
        var subject = new Coordinate(coordinateX, coordinateY);

        var result = subject.Move(new Velocity(velocityX, velocityY), elapsedSeconds);
        
        VectorAssert.Equal(new(expectedX, expectedY), result);
    }
}
