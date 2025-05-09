using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class VelocityExtensionsTests {
    [Theory]
    [InlineData(10, 10, 3, 4, 1, 13, 14)]
    [InlineData(10, 10, -3, -4, 1, 7, 6)]
    [InlineData(10, 10, 3, 4, 0.5, 11.5, 12)]
    [InlineData(10, 10, 3, 4, 2.5, 17.5, 20)]
    public void Accelerate(double velocityX, double velocityY, double accelerationX, double accelerationY, double elapsedSeconds, double expectedX, double expectedY) {
        var subject = new Velocity(velocityX, velocityY);
        
        var result = subject.Accelerate(new(accelerationX, accelerationY), elapsedSeconds);

        Assert.Equal(expectedX, result.X, 0.001);
        Assert.Equal(expectedY, result.Y, 0.001);
    }

    [Theory]
    [InlineData(100, 200, 5, 20, 40)]
    [InlineData(-200, -100, 2, -100, -50)]
    public void GetAcceleration(double velocityX, double velocityY, double elapsedSeconds, double expectedX, double expectedY) {
        var subject = new Velocity(velocityX, velocityY);
        
        var result = subject.GetAcceleration(elapsedSeconds);
    
        Assert.Equal(expectedX, result.X, 0.001);
        Assert.Equal(expectedY, result.Y, 0.001);
    }
}
