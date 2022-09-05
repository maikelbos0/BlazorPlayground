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

            Assert.Equal(Canvas.MinimumWidth, canvas.Width);
        }

        [Fact]
        public void Height_Minimum() {
            var canvas = new Canvas() {
                Height = 0
            };

            Assert.Equal(Canvas.MinimumHeight, canvas.Height);
        }

        [Fact]
        public void GridSize_Minimum() {
            var canvas = new Canvas() {
                GridSize = 0
            };

            Assert.Equal(Canvas.MinimumGridSize, canvas.GridSize);
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
        public void StartActionExecution() {
            var canvas = new Canvas();

            canvas.StartActionExecution(new Point(100, 150));

            PointAssert.Equal(new Point(100, 150), canvas.StartPoint);
        }

        [Fact]
        public void UpdateActionExecution_Null_StartPoint() {
            var canvas = new Canvas();

            canvas.UpdateActionExecution(new Point(200, 250));

            Assert.Null(canvas.EndPoint);
        }

        [Fact]
        public void UpdateActionExecution() {
            var canvas = new Canvas() {
                StartPoint = new Point(100, 150)
            };

            canvas.UpdateActionExecution(new Point(200, 250));

            PointAssert.Equal(new Point(200, 250), canvas.EndPoint);
        }

        [Fact]
        public void EndActionExecution_Null_StartPoint() {
            var canvas = new Canvas() {
                IsDrawing = true
            };

            canvas.EndActionExecution();

            Assert.Empty(canvas.Shapes);
        }

        [Fact]
        public void EndActionExecution_Null_EndPoint() {
            var canvas = new Canvas() {
                StartPoint = new Point(100, 150),
                IsDrawing = true
            };

            canvas.EndActionExecution();

            Assert.Empty(canvas.Shapes);
        }

        [Fact]
        public void EndActionExecution_IsDrawing() {
            var canvas = new Canvas() {
                StartPoint = new Point(100, 150),
                EndPoint = new Point(200, 250),
                IsDrawing = true
            };

            canvas.EndActionExecution();

            Assert.Single(canvas.Shapes);
        }

        [Fact]
        public void EndActionExecution_SelectedShape() {
            var shape = new Rectangle(new Point(100, 150), new Point(200, 250));
            var canvas = new Canvas() {
                StartPoint = new Point(100, 150),
                EndPoint = new Point(200, 250),
                IsDrawing = false,
                SelectedShape = shape
            };

            canvas.EndActionExecution();

            Assert.Equal(new Point(200, 250), shape.StartPoint);
            Assert.Equal(new Point(300, 350), shape.EndPoint);
        }

        [Fact]
        public void EndActionExecution_SelectedAnchor() {
            var shape = new Rectangle(new Point(100, 150), new Point(200, 250));
            var canvas = new Canvas() {
                StartPoint = new Point(100, 150),
                EndPoint = new Point(200, 250),
                IsDrawing = false,
                SelectedShape = shape,
                SelectedAnchor = shape.Anchors[0]
            };

            canvas.EndActionExecution();

            Assert.Equal(new Point(200, 250), shape.StartPoint);
            Assert.Equal(new Point(200, 250), shape.EndPoint);
        }

        [Fact]
        public void EndActionExecution_IsSelecting() {
            var shape = new Rectangle(new Point(100, 150), new Point(200, 250));
            var canvas = new Canvas() {
                SelectedShape = shape,
                IsSelecting = true
            };

            canvas.EndActionExecution();

            Assert.Same(shape, canvas.SelectedShape);
        }

        [Fact]
        public void EndActionExecution_Not_IsSelecting() {
            var canvas = new Canvas() {
                SelectedShape = new Rectangle(new Point(100, 150), new Point(200, 250))
            };

            canvas.EndActionExecution();

            Assert.Null(canvas.SelectedShape);
        }

        [Fact]
        public void EndActionExecution_Clears() {
            var shape = new Line(new Point(100, 100), new Point(200, 200));
            var canvas = new Canvas() {
                StartPoint = new Point(150, 177),
                EndPoint = new Point(150, 202),
                SelectedShape = shape,
                IsSelecting = true,
                SelectedAnchor = shape.Anchors[0]
            };

            canvas.EndActionExecution();

            Assert.Null(canvas.StartPoint);
            Assert.Null(canvas.EndPoint);
            Assert.False(canvas.IsSelecting);
            Assert.Null(canvas.SelectedAnchor);
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

            canvas.DrawSettings.Opacity = 45;
            canvas.DrawSettings.FillPaintManager.Mode = PaintMode.Color;
            canvas.DrawSettings.FillPaintManager.ColorValue = "yellow";
            canvas.DrawSettings.StrokeLinecap = Linecap.Round;
            canvas.DrawSettings.StrokeLinejoin = Linejoin.Round;
            canvas.DrawSettings.StrokePaintManager.Mode = PaintMode.Color;
            canvas.DrawSettings.StrokePaintManager.ColorValue = "red";
            canvas.DrawSettings.StrokeWidth = 5;

            var shape = canvas.CreateShape();

            Assert.NotNull(shape);
            Assert.Equal(45, shape!.Opacity);
            PaintServerAssert.Equal(new Color(255, 255, 0, 1), shape.Fill);
            Assert.Equal(Linecap.Round, shape.StrokeLinecap);
            Assert.Equal(Linejoin.Round, shape.StrokeLinejoin);
            PaintServerAssert.Equal(new Color(255, 0, 0, 1), shape.Stroke);
            Assert.Equal(5, shape.StrokeWidth);
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
        public void ApplyOpacityToSelectedShape() {
            var canvas = new Canvas() {
                SelectedShape = new Line(new Point(100, 100), new Point(200, 200)),
            };

            canvas.DrawSettings.Opacity = 45;

            canvas.ApplyOpacityToSelectedShape();

            Assert.Equal(45, canvas.SelectedShape.Opacity);
        }

        [Fact]
        public void ApplyFillToSelectedShape() {
            var canvas = new Canvas() {
                SelectedShape = new Line(new Point(100, 100), new Point(200, 200)),
            };

            canvas.DrawSettings.FillPaintManager.Mode = PaintMode.Color;
            canvas.DrawSettings.FillPaintManager.ColorValue = "yellow";

            canvas.ApplyFillToSelectedShape();

            PaintServerAssert.Equal(new Color(255, 255, 0, 1), canvas.SelectedShape.Fill);
        }

        [Fact]
        public void ApplyStrokeToSelectedShape() {
            var canvas = new Canvas() {
                SelectedShape = new Rectangle(new Point(100, 100), new Point(200, 200)),
            };

            canvas.DrawSettings.StrokePaintManager.Mode = PaintMode.Color;
            canvas.DrawSettings.StrokePaintManager.ColorValue = "yellow";

            canvas.ApplyStrokeToSelectedShape();

            PaintServerAssert.Equal(new Color(255, 255, 0, 1), canvas.SelectedShape.Stroke);
        }

        [Fact]
        public void ApplyStrokeLinecapToSelectedShape() {
            var canvas = new Canvas() {
                SelectedShape = new Line(new Point(100, 100), new Point(200, 200)),
            };

            canvas.DrawSettings.StrokeLinecap = Linecap.Round;

            canvas.ApplyStrokeLinecapToSelectedShape();

            Assert.Equal(Linecap.Round, canvas.SelectedShape.StrokeLinecap);
        }

        [Fact]
        public void ApplyStrokeLinejoinToSelectedShape() {
            var canvas = new Canvas() {
                SelectedShape = new Rectangle(new Point(100, 100), new Point(200, 200)),
            };

            canvas.DrawSettings.StrokeLinejoin = Linejoin.Round;

            canvas.ApplyStrokeLinejoinToSelectedShape();

            Assert.Equal(Linejoin.Round, canvas.SelectedShape.StrokeLinejoin);
        }

        [Fact]
        public void ApplyStrokeWidthToSelectedShape() {
            var canvas = new Canvas() {
                SelectedShape = new Line(new Point(100, 100), new Point(200, 200)),
            };

            canvas.DrawSettings.StrokeWidth = 5;

            canvas.ApplyStrokeWidthToSelectedShape();

            Assert.Equal(5, canvas.SelectedShape.StrokeWidth);
        }

        [Fact]
        public void ApplySidesToSelectedShape() {
            var canvas = new Canvas() {
                SelectedShape = new RegularPolygon(new Point(100, 100), new Point(200, 200)),
            };

            canvas.DrawSettings.Sides = 5;

            canvas.ApplySidesToSelectedShape();

            Assert.Equal(5, canvas.SelectedShape.Sides);
        }

        [Fact]
        public void MoveSelectedShapeToBack() {
            var shapes = Enumerable.Range(0, 5).Select(i => new Line(new Point(100, 100), new Point(200, 200))).ToList();
            var canvas = new Canvas();

            canvas.Shapes.AddRange(shapes);
            canvas.SelectedShape = shapes[2];

            canvas.MoveSelectedShapeToBack();

            Assert.Same(shapes[0], canvas.Shapes[1]);
            Assert.Same(shapes[1], canvas.Shapes[2]);
            Assert.Same(shapes[2], canvas.Shapes[0]);
            Assert.Same(shapes[3], canvas.Shapes[3]);
            Assert.Same(shapes[4], canvas.Shapes[4]);
        }

        [Fact]
        public void MoveSelectedShapeBackward() {
            var shapes = Enumerable.Range(0, 5).Select(i => new Line(new Point(100, 100), new Point(200, 200))).ToList();
            var canvas = new Canvas();

            canvas.Shapes.AddRange(shapes);
            canvas.SelectedShape = shapes[2];

            canvas.MoveSelectedShapeBackward();

            Assert.Same(shapes[0], canvas.Shapes[0]);
            Assert.Same(shapes[1], canvas.Shapes[2]);
            Assert.Same(shapes[2], canvas.Shapes[1]);
            Assert.Same(shapes[3], canvas.Shapes[3]);
            Assert.Same(shapes[4], canvas.Shapes[4]);
        }

        [Fact]
        public void MoveSelectedShapeForward() {
            var shapes = Enumerable.Range(0, 5).Select(i => new Line(new Point(100, 100), new Point(200, 200))).ToList();
            var canvas = new Canvas();

            canvas.Shapes.AddRange(shapes);
            canvas.SelectedShape = shapes[2];

            canvas.MoveSelectedShapeForward();

            Assert.Same(shapes[0], canvas.Shapes[0]);
            Assert.Same(shapes[1], canvas.Shapes[1]);
            Assert.Same(shapes[2], canvas.Shapes[3]);
            Assert.Same(shapes[3], canvas.Shapes[2]);
            Assert.Same(shapes[4], canvas.Shapes[4]);
        }

        [Fact]
        public void MoveSelectedShapeToFront() {
            var shapes = Enumerable.Range(0, 5).Select(i => new Line(new Point(100, 100), new Point(200, 200))).ToList();
            var canvas = new Canvas();

            canvas.Shapes.AddRange(shapes);
            canvas.SelectedShape = shapes[2];

            canvas.MoveSelectedShapeToFront();

            Assert.Same(shapes[0], canvas.Shapes[0]);
            Assert.Same(shapes[1], canvas.Shapes[1]);
            Assert.Same(shapes[2], canvas.Shapes[4]);
            Assert.Same(shapes[3], canvas.Shapes[2]);
            Assert.Same(shapes[4], canvas.Shapes[3]);
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
