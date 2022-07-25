using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class PointTests {
        [Fact]
        public void Add() {
            var result = new Point(100, 150) + new Point(200, 250);

            PointAssert.Equal(new Point(300, 400), result);
        }

        [Fact]
        public void Subtract() {
            var result = new Point(100, 150) - new Point(200, 100);

            PointAssert.Equal(new Point(-100, 50), result);
        }

        [Fact]
        public void Multiply() {
            var result = new Point(100, 150) * 2.5;

            PointAssert.Equal(new Point(250, 375), result);
        }

        [Fact]
        public void Divide() {
            var result = new Point(100, 150) / 2.5;

            PointAssert.Equal(new Point(40, 60), result);
        }

        [Fact]
        public void Double() {
            var result = new Point(30, 40).Distance;

            Assert.Equal(50, result, 1);
        }

        [Theory]
        [InlineData(100, 100, 200, 200, 150, 150, true)]
        [InlineData(200, 200, 100, 100, 150, 150, true)]
        [InlineData(100, 200, 200, 100, 150, 150, true)]
        [InlineData(200, 100, 100, 200, 150, 150, true)]
        [InlineData(100, 100, 200, 200, 250, 150, false)]
        [InlineData(100, 100, 200, 200, 50, 150, false)]
        [InlineData(100, 100, 200, 200, 150, 250, false)]
        [InlineData(100, 100, 200, 200, 150, 50, false)]
        public void IsContained(double boundaryX1, double boundaryY1, double boundaryX2, double boundaryY2, double x, double y, bool expectedIsContained) {
            var boundaryPoint1 = new Point(boundaryX1, boundaryY1);
            var boundaryPoint2 = new Point(boundaryX2, boundaryY2);
            var point = new Point(x, y);

            Assert.Equal(expectedIsContained, point.IsContainedBy(boundaryPoint1, boundaryPoint2));
        }

        [Theory]
        [InlineData(100, 200, 50, 100, 200)]
        [InlineData(101, 201, 50, 100, 200)]
        [InlineData(124.9, 224.9, 50, 100, 200)]
        [InlineData(101, 199, 50, 100, 200)]
        [InlineData(124.9, 175.1, 50, 100, 200)]
        [InlineData(99, 201, 50, 100, 200)]
        [InlineData(76.1, 224.9, 50, 100, 200)]
        [InlineData(99, 199, 50, 100, 200)]
        [InlineData(76.1, 175.1, 50, 100, 200)]
        public void SnapToGrid(double x, double y, int gridSize, double expectedX, double expectedY) {
            var point = new Point(x, y);

            PointAssert.Equal(new Point(expectedX, expectedY), point.SnapToGrid(gridSize));
        }
    }
}
