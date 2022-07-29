using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class CanvasTests {
        [Fact]
        public void MinimumWidth() {
            var canvas = new Canvas() {
                Width = 0
            };

            Assert.Equal(1, canvas.Width);
        }

        [Fact]
        public void MinimumHeight() {
            var canvas = new Canvas() {
                Height = 0
            };

            Assert.Equal(1, canvas.Height);
        }

        [Fact]
        public void MinimumGridSize() {
            var canvas = new Canvas() {
                GridSize = 0
            };

            Assert.Equal(1, canvas.GridSize);
        }

        [Theory]
        [InlineData(10, false, 12, 23)]
        [InlineData(10, true, 10, 20)]
        [InlineData(25, true, 0, 25)]
        public void SnappedStartPoint(int gridSize, bool snapToGrid, double expectedX, double expectedY) {
            var canvas = new Canvas() {
                StartPoint = new Point(12, 23),
                GridSize = gridSize,
                SnapToGrid = snapToGrid
            };

            Assert.NotNull(canvas.SnappedStartPoint);
            PointAssert.Equal(new Point(expectedX, expectedY), canvas.SnappedStartPoint!);
        }

        [Fact]
        public void NullSnappedStartPoint() {
            var canvas = new Canvas();

            Assert.Null(canvas.SnappedStartPoint);
        }

        [Theory]
        [InlineData(10, false, 12, 23)]
        [InlineData(10, true, 10, 20)]
        [InlineData(25, true, 0, 25)]
        public void SnappedEndPoint(int gridSize, bool snapToGrid, double expectedX, double expectedY) {
            var canvas = new Canvas() {
                EndPoint = new Point(12, 23),
                GridSize = gridSize,
                SnapToGrid = snapToGrid
            };

            Assert.NotNull(canvas.SnappedEndPoint);
            PointAssert.Equal(new Point(expectedX, expectedY), canvas.SnappedEndPoint!);
        }

        [Fact]
        public void NullSnappedEndPoint() {
            var canvas = new Canvas();

            Assert.Null(canvas.SnappedEndPoint);
        }

        [Fact]
        public void IsDragging() {
            var canvas = new Canvas() {
                StartPoint = new Point(25, 25),
                EndPoint = new Point(50, 50)
            };

            Assert.True(canvas.IsDragging);
        }

        [Fact]
        public void IsDraggingForNullStartPoint() {
            var canvas = new Canvas() {
                EndPoint = new Point(50, 50)
            };

            Assert.False(canvas.IsDragging);
        }

        [Fact]
        public void IsDraggingForNullEndPoint() {
            var canvas = new Canvas() {
                StartPoint = new Point(25, 25)
            };

            Assert.False(canvas.IsDragging);
        }

        [Fact]
        public void IsDraggingForNullStartPointEndPoint() {
            var canvas = new Canvas();

            Assert.False(canvas.IsDragging);
        }

        [Fact]
        public void Delta() {
            var canvas = new Canvas() {
                StartPoint = new Point(25, 25),
                EndPoint = new Point(50, 50)
            };

            Assert.NotNull(canvas.Delta);
            PointAssert.Equal(new Point(25, 25), canvas.Delta!);
        }

        [Fact]
        public void DeltaForNullStartPoint() {
            var canvas = new Canvas() {
                EndPoint = new Point(50, 50)
            };

            Assert.Null(canvas.Delta);
        }

        [Fact]
        public void DeltaForNullEndPoint() {
            var canvas = new Canvas() {
                StartPoint = new Point(25, 25)
            };

            Assert.Null(canvas.Delta);
        }

        [Fact]
        public void DeltaForNullStartPointEndPoint() {
            var canvas = new Canvas();

            Assert.Null(canvas.Delta);
        }

        [Fact]
        public void ExportSvg() {
            var canvas = new Canvas() {
                Width = 600,
                Height = 400,
                Shapes = {
                    new Line(new Point(100, 150), new Point(200, 250)),
                    new Circle(new Point(30, 50), new Point(30, 70))
                }
            };

            var result = canvas.ExportSvg();
            var shapes = result.Elements().ToList();

            Assert.Equal("svg", result.Name);
            Assert.Equal("600", result.Attribute("width")?.Value);
            Assert.Equal("400", result.Attribute("height")?.Value);
            Assert.Equal("0 0 600 400", result.Attribute("viewBox")?.Value);
            Assert.Equal(2, shapes.Count);
            Assert.Single(shapes, s => s.Name == "line");
            Assert.Single(shapes, s => s.Name == "circle");
        }
    }
}
