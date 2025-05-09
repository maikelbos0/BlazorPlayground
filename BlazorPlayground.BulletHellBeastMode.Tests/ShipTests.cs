using System;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class ShipTests {
    [Fact]
    public void Create() {
        var position = new Coordinate(500, 900);
        var sections = new List<GameElementSection>();
        var result = Ship.Create(new(500, 900), sections);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(position, result.Position);
        Assert.Equal(sections, result.Sections);
    }

    [Theory]
    [InlineData(Direction.None, 0, 0)]
    [InlineData(Direction.Up, 0, -1000)]
    [InlineData(Direction.Down, 0, 1000)]
    [InlineData(Direction.Left, -1000, 0)]
    [InlineData(Direction.Right, 1000, 0)]
    [InlineData(Direction.Up | Direction.Left, -707.1068, -707.1068)]
    [InlineData(Direction.Down | Direction.Right, 707.1068, 707.1068)]
    [InlineData(Direction.Up | Direction.Down | Direction.Left | Direction.Right, 0, 0)]
    public void GetDirectionalVelocity(Direction direction, double expectedX, double expectedY) {
        var subject = Ship.Create(new(500, 900), []);

        subject.Direction = direction;

        var result = subject.GetDirectionalVelocity();

        Assert.Equal(expectedX, result.X, 0.001);
        Assert.Equal(expectedY, result.Y, 0.001);
    }

    [Fact]
    public void GetVelocityFromTargetPositionWhenNull() {
        var subject = Ship.Create(new(500, 900), []);

        subject.TargetPosition = null;

        var result = subject.GetVelocityFromTargetPosition();

        Assert.Equal(new(0, 0), result);
    }

    [Theory]
    [InlineData(1000, 1000, 1000, 1000, 0, 0)]
    [InlineData(1000, 1000, 1500, 1000, 1000, 0)]
    [InlineData(1000, 1000, 1000, 500, 0, -1000)]
    [InlineData(1000, 1000, 1300, 1400, 600, 800)]
    [InlineData(1000, 1000, 600, 700, -800, -600)]
    [InlineData(1000, 1000, 900, 925, -282.843, -212.132)]
    public void GetVelocityFromTargetPosition(double positionX, double positionY, double targetPositionX, double targetPositionY, double expectedX, double expectedY) {
        var subject = Ship.Create(new(positionX, positionY), []);

        subject.TargetPosition = new(targetPositionX, targetPositionY);

        var result = subject.GetVelocityFromTargetPosition();

        Assert.Equal(expectedX, result.X, 0.001);
        Assert.Equal(expectedY, result.Y, 0.001);
    }

    [Theory]
    [InlineData(0, 0, 0, 0, 0, 0)]
    [InlineData(0, 0, 1000, 1000, 1000, 1000)]
    [InlineData(0, 0, 2000, 2000, 1414.214, 1414.214)]
    [InlineData(1000, 1000, 0, 0, 0, 0)]
    [InlineData(1000, 1000, -1000, -1000, -1000, -1000)]
    [InlineData(1000, 1000, -2000, -2000, -1414.214, -1414.214)]
    [InlineData(100, 100, -1000, -1000, -282.843, -282.843)]
    [InlineData(900, 900, 1000, 1000, 282.843, 282.843)]
    public void AdjustVelocityToBounds(double positionX, double positionY, double velocityX, double velocityY, double expectedX, double expectedY) {
        var subject = Ship.Create(new(positionX, positionY), []);

        var result = subject.AdjustVelocityToBounds(new Velocity(velocityX, velocityY));

        Assert.Equal(expectedX, result.X, 0.001);
        Assert.Equal(expectedY, result.Y, 0.001);
    }
}
