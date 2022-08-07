using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class CanvasTests {
        [Fact]
        public void Width_Minimum() {
            var canvas = new Canvas() {
                Width = 0
            };

            Assert.Equal(1, canvas.Width);
        }

        [Fact]
        public void Height_Minimum() {
            var canvas = new Canvas() {
                Height = 0
            };

            Assert.Equal(1, canvas.Height);
        }

        [Fact]
        public void GridSize_Minimum() {
            var canvas = new Canvas() {
                GridSize = 0
            };

            Assert.Equal(1, canvas.GridSize);
        }

        [Fact]
        public void CurrentShapeDefinition_IsDrawing() {
            var canvas = new Canvas() {
                IsDrawing = true,
                CurrentShapeDefinition = ShapeDefinition.Get(typeof(Circle))
            };

            Assert.Same(ShapeDefinition.Get(typeof(Circle)), canvas.CurrentShapeDefinition);
        }

        [Fact]
        public void CurrentShapeDefinition_SelectedShape() {
            var canvas = new Canvas() {
                IsDrawing = false,
                CurrentShapeDefinition = ShapeDefinition.Get(typeof(Circle)),
                SelectedShape = new Rectangle(new Point(100, 150), new Point(200, 250))
            };

            Assert.Same(ShapeDefinition.Get(typeof(Rectangle)), canvas.CurrentShapeDefinition);
        }

        [Fact]
        public void CurrentShapeDefinition_Not_IsDrawing_No_SelectedShape() {
            var canvas = new Canvas() {
                IsDrawing = false
            };

            Assert.Same(ShapeDefinition.None, canvas.CurrentShapeDefinition);
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
        public void SnappedStartPoint_Null() {
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
        public void SnappedEndPoint_Null() {
            var canvas = new Canvas();

            Assert.Null(canvas.SnappedEndPoint);
        }

        [Fact]
        public void IsExecutingAction() {
            var canvas = new Canvas() {
                StartPoint = new Point(25, 25),
                EndPoint = new Point(50, 50)
            };

            Assert.True(canvas.IsExecutingAction);
        }

        [Fact]
        public void IsExecutingAction_Null_StartPoint() {
            var canvas = new Canvas() {
                EndPoint = new Point(50, 50)
            };

            Assert.False(canvas.IsExecutingAction);
        }

        [Fact]
        public void IsExecutingAction_Null_EndPoint() {
            var canvas = new Canvas() {
                StartPoint = new Point(25, 25)
            };

            Assert.False(canvas.IsExecutingAction);
        }

        [Fact]
        public void IsExecutingAction_Null_StartPoint_And_EndPoint() {
            var canvas = new Canvas();

            Assert.False(canvas.IsExecutingAction);
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
        public void Delta_Null_StartPoint() {
            var canvas = new Canvas() {
                EndPoint = new Point(50, 50)
            };

            Assert.Null(canvas.Delta);
        }

        [Fact]
        public void Delta_Null_EndPoint() {
            var canvas = new Canvas() {
                StartPoint = new Point(25, 25)
            };

            Assert.Null(canvas.Delta);
        }

        [Fact]
        public void Delta_Null_StartPoint_And_EndPoint() {
            var canvas = new Canvas();

            Assert.Null(canvas.Delta);
        }

        [Fact]
        public void StartDrawing() {
            var canvas = new Canvas() {
                IsDrawing = false,
                CurrentShapeDefinition = ShapeDefinition.Get(typeof(Circle)),
                SelectedShape = new Line(new Point(100, 200), new Point(150, 250))
            };

            canvas.StartDrawing(ShapeDefinition.Get(typeof(Rectangle)));

            Assert.True(canvas.IsDrawing);
            Assert.Equal(ShapeDefinition.Get(typeof(Rectangle)), canvas.CurrentShapeDefinition);
            Assert.Null(canvas.SelectedShape);
        }

        [Fact]
        public void StopDrawing() {
            var canvas = new Canvas() {
                IsDrawing = true,
                CurrentShapeDefinition = ShapeDefinition.Get(typeof(Circle))
            };

            canvas.StopDrawing();

            Assert.False(canvas.IsDrawing);
        }

        [Fact]
        public void AddShape_Without_AutoSelect() {
            var canvas = new Canvas() {
                StartPoint = new Point(100, 200),
                EndPoint = new Point(150, 250)
            };

            canvas.StartDrawing(ShapeDefinition.Values.Single(d => d.Name == "Line"));

            canvas.AddShape();

            Assert.Single(canvas.Shapes);
            Assert.Null(canvas.SelectedShape);
            Assert.True(canvas.IsDrawing);
        }

        [Fact]
        public void AddShape_With_AutoSelect() {
            var canvas = new Canvas() {
                StartPoint = new Point(100, 200),
                EndPoint = new Point(150, 250)
            };

            canvas.StartDrawing(ShapeDefinition.Values.Single(d => d.Name == "Quadratic bezier"));

            canvas.AddShape();

            Assert.Equal(Assert.Single(canvas.Shapes), canvas.SelectedShape);
            Assert.False(canvas.IsDrawing);
        }

        [Fact]
        public void CreateShape_Null_StartPoint() {
            var canvas = new Canvas() {
                EndPoint = new Point(150, 250)
            };

            Assert.Null(canvas.CreateShape());
        }

        [Fact]
        public void CreateShape_Null_EndPoint() {
            var canvas = new Canvas() {
                StartPoint = new Point(150, 250)
            };

            Assert.Null(canvas.CreateShape());
        }

        [Fact]
        public void CreateShape() {
            var canvas = new Canvas() {
                StartPoint = new Point(150, 250),
                EndPoint = new Point(150, 250)
            };

            canvas.DrawSettings.FillPaintManager.Mode = PaintMode.Color;
            canvas.DrawSettings.FillPaintManager.ColorValue = "yellow";
            canvas.DrawSettings.StrokeLinecap = Linecap.Round;
            canvas.DrawSettings.StrokeLinejoin = Linejoin.Round;
            canvas.DrawSettings.StrokePaintManager.Mode = PaintMode.Color;
            canvas.DrawSettings.StrokePaintManager.ColorValue = "red";
            canvas.DrawSettings.StrokeWidth = 5;

            var shape = canvas.CreateShape();

            Assert.NotNull(shape);
            PaintServerAssert.Equal(new Color(255, 255, 0, 1), shape!.Fill);
            Assert.Equal(Linecap.Round, shape.StrokeLinecap);
            Assert.Equal(Linejoin.Round, shape.StrokeLinejoin);
            PaintServerAssert.Equal(new Color(255, 0, 0, 1), shape.Stroke);
            Assert.Equal(5, shape.StrokeWidth);
        }
        
        [Fact]
        public void CreateVirtualSelectedShape_Shape() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                StartPoint = new Point(150, 150),
                EndPoint = new Point(177, 202),
                SnapToGrid = true,
                GridSize = 25,
                SelectedShape = shape
            };

            var virtualShape = Assert.IsType<Line>(canvas.CreateVirtualSelectedShape());

            Assert.NotSame(shape, virtualShape);
            Assert.Equal(new Point(100, 100), shape.StartPoint);
            Assert.Equal(new Point(200, 200), shape.EndPoint);
            Assert.Equal(new Point(125, 150), virtualShape.StartPoint);
            Assert.Equal(new Point(225, 250), virtualShape.EndPoint);
        }
        
        [Fact]
        public void CreateVirtualSelectedShape_Anchor() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                StartPoint = new Point(150, 150),
                EndPoint = new Point(177, 202),
                SnapToGrid = true,
                GridSize = 25,
                SelectedShape = shape,
                SelectedAnchor = shape.Anchors[0]
            };

            var virtualShape = Assert.IsType<Line>(canvas.CreateVirtualSelectedShape());

            Assert.NotSame(shape, virtualShape);
            Assert.Equal(new Point(100, 100), shape.StartPoint);
            Assert.Equal(new Point(200, 200), shape.EndPoint);
            Assert.Equal(new Point(175, 200), virtualShape.StartPoint);
            Assert.Equal(new Point(200, 200), virtualShape.EndPoint);
        }
        
        [Fact]
        public void CreateVirtualSelectedShape_Throws_For_Null_SelectedShape() {
            var canvas = new Canvas() {
                StartPoint = new Point(150, 150),
                EndPoint = new Point(177, 202)
            };

            Assert.Throws<InvalidOperationException>(() => canvas.CreateVirtualSelectedShape());
        }
        
        [Fact]
        public void CreateVirtualSelectedShape_Throws_For_Null_StartPoint() {
            var canvas = new Canvas() {
                EndPoint = new Point(177, 202),
                SelectedShape = new Line(new Point(100, 100), new Point(200, 200))
            };

            Assert.Throws<InvalidOperationException>(() => canvas.CreateVirtualSelectedShape());
        }
        
        [Fact]
        public void CreateVirtualSelectedShape_Throws_For_Null_EndPoint() {
            var canvas = new Canvas() {
                StartPoint = new Point(150, 150),
                SelectedShape = new Line(new Point(100, 100), new Point(200, 200))
            };

            Assert.Throws<InvalidOperationException>(() => canvas.CreateVirtualSelectedShape());
        }

        [Fact]
        public void TransformSelectedShape() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                StartPoint = new Point(150, 150),
                EndPoint = new Point(177, 202),
                SnapToGrid = true,
                GridSize = 25,
                SelectedShape = shape
            };

            canvas.TransformSelectedShape();

            PointAssert.Equal(new Point(125, 150), shape.StartPoint);
            PointAssert.Equal(new Point(225, 250), shape.EndPoint);
        }

        [Fact]
        public void TransformSelectedShape_Null_SelectedShape() {
            var canvas = new Canvas() {
                StartPoint = new Point(150, 177),
                EndPoint = new Point(150, 202)
            };

            canvas.TransformSelectedShape();
        }

        [Fact]
        public void TransformSelectedShape_Null_Delta() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                SelectedShape = shape
            };

            canvas.TransformSelectedShape();

            PointAssert.Equal(new Point(100, 100), shape.StartPoint);
            PointAssert.Equal(new Point(200, 200), shape.EndPoint);
        }

        [Fact]
        public void TransformSelectedShape_Clears() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                StartPoint = new Point(150, 177),
                EndPoint = new Point(150, 202),
                SelectedShape = shape,
                SelectedAnchor = shape.Anchors[0]
            };

            canvas.TransformSelectedShape();

            Assert.Null(canvas.StartPoint);
            Assert.Null(canvas.EndPoint);
            Assert.Null(canvas.SelectedAnchor);
        }

        [Fact]
        public void TransformSelectedShapeAnchor() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                EndPoint = new Point(127, 152),
                SnapToGrid = true,
                GridSize = 25,
                SelectedShape = shape,
                SelectedAnchor = shape.Anchors[0]
            };

            canvas.TransformSelectedShapeAnchor();

            PointAssert.Equal(new Point(125, 150), shape.StartPoint);
            PointAssert.Equal(new Point(200, 200), shape.EndPoint);
        }

        [Fact]
        public void TransformSelectedShapeAnchor_Null_SelectedShape() {
            var canvas = new Canvas() {
                EndPoint = new Point(177, 202)
            };

            canvas.TransformSelectedShapeAnchor();
        }

        [Fact]
        public void TransformSelectedShapeAnchor_Null_SelectedAnchor() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                EndPoint = new Point(177, 202)
            };

            canvas.TransformSelectedShapeAnchor();

            PointAssert.Equal(new Point(100, 100), shape.StartPoint);
            PointAssert.Equal(new Point(200, 200), shape.EndPoint);
        }

        [Fact]
        public void TransformSelectedShapeAnchor_Null_EndPoint() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                SelectedAnchor = shape.Anchors[0]
            };

            canvas.TransformSelectedShapeAnchor();

            PointAssert.Equal(new Point(100, 100), shape.StartPoint);
            PointAssert.Equal(new Point(200, 200), shape.EndPoint);
        }

        [Fact]
        public void TransformSelectedShapeAnchor_Clears() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                EndPoint = new Point(127, 152),
                SelectedShape = shape,
                SelectedAnchor = shape.Anchors[0]
            };

            canvas.TransformSelectedShapeAnchor();

            Assert.Null(canvas.StartPoint);
            Assert.Null(canvas.EndPoint);
            Assert.Null(canvas.SelectedAnchor);
        }

        [Fact]
        public void DeleteSelectedShape() {
            var selectedShape = new Line(new Point(100, 100), new Point(200, 200));
            var shape = new Line(new Point(200, 200), new Point(100, 200));
            var canvas = new Canvas() {
                Shapes = new List<Shape>() { 
                    selectedShape,
                    shape
                },
                SelectedShape = selectedShape
            };

            canvas.DeleteSelectedShape();

            Assert.Null(canvas.SelectedShape);
            Assert.Equal(shape, Assert.Single(canvas.Shapes));
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
