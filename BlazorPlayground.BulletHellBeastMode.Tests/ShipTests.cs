using System;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class ShipTests {
    [Fact]
    public void Constructor() {
        var position = new Coordinate(500, 900);
        var sections = new List<GameElementSection>();
        var result = new Ship(new(500, 900), sections);

        Assert.NotEqual(Guid.Empty, result.Id);
        VectorAssert.Equal(position, result.Position);
        Assert.Equal(sections, result.Sections);
    }

    [Theory]
    [InlineData(Direction.None, 0, 0)]
    [InlineData(Direction.Up, 0, -1000)]
    [InlineData(Direction.Down, 0, 1000)]
    [InlineData(Direction.Left, -1000, 0)]
    [InlineData(Direction.Right, 1000, 0)]
    [InlineData(Direction.Up | Direction.Left, -707.1, -707.1)]
    [InlineData(Direction.Down | Direction.Right, 707.1, 707.1)]
    [InlineData(Direction.Up | Direction.Down | Direction.Left | Direction.Right, 0, 0)]
    public void GetDirectionalVelocity(Direction direction, double expectedX, double expectedY) {
        var subject = new Ship(new(500, 900), []) {
            Direction = direction
        };

        var result = subject.GetDirectionalVelocity();

        VectorAssert.Equal(new(expectedX, expectedY), result);
    }

    [Fact]
    public void GetVelocityFromTargetPositionWhenNull() {
        var subject = new Ship(new(500, 900), []) {
            TargetPosition = null
        };

        var result = subject.GetVelocityFromTargetPosition();

        Assert.Equal(new(0, 0), result);
    }

    [Theory]
    [InlineData(1000, 1000, 1000, 1000, 0, 0)]
    [InlineData(1000, 1000, 1500, 1000, 1000, 0)]
    [InlineData(1000, 1000, 1000, 500, 0, -1000)]
    [InlineData(1000, 1000, 1300, 1400, 600, 800)]
    [InlineData(1000, 1000, 600, 700, -800, -600)]
    [InlineData(1000, 1000, 900, 925, -282.8, -212.1)]
    public void GetVelocityFromTargetPosition(double positionX, double positionY, double targetPositionX, double targetPositionY, double expectedX, double expectedY) {
        var subject = new Ship(new(positionX, positionY), []) {
            TargetPosition = new(targetPositionX, targetPositionY)
        };

        var result = subject.GetVelocityFromTargetPosition();

        VectorAssert.Equal(new(expectedX, expectedY), result);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0, 0, 1000, 1000, 1000, 1000)]
    [InlineData(0, 0, 2000, 2000, 1414.2, 1414.2)]
    [InlineData(1000, 1000, 0, 0, 0, 0)]
    [InlineData(1000, 1000, -1000, -1000, -1000, -1000)]
    [InlineData(1000, 1000, -2000, -2000, -1414.2, -1414.2)]
    [InlineData(100, 100, -1000, -1000, -282.8, -282.8)]
    [InlineData(900, 900, 1000, 1000, 282.8, 282.8)]
    public void AdjustVelocityToBounds(double positionX, double positionY, double velocityX, double velocityY, double expectedX, double expectedY) {
        var subject = new Ship(new(positionX, positionY), []);

        var result = subject.AdjustVelocityToBounds(new Velocity(velocityX, velocityY));

        VectorAssert.Equal(new(expectedX, expectedY), result);
    }

    [Fact]
    public void ProcessElapsedTimeWhenStationary() {
        var subject = new Ship(new(500, 900), []);

        var result = subject.ProcessElapsedTime(0.1);

        Assert.False(result);
        VectorAssert.Equal(new(500, 900), subject.Position);
    }

    [Fact]
    public void ProcessElapsedTime() {
        var subject = new Ship(new(500, 900), []) {
            Direction = Direction.Left | Direction.Up,
            TargetPosition = new(1000, 500)
        };

        var result = subject.ProcessElapsedTime(0.1);

        Assert.True(result);
        VectorAssert.Equal(new(11.1, -199.7), subject.Velocity);
        VectorAssert.Equal(new(501.1, 880), subject.Position);
    }
}
