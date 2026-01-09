using Xunit;

namespace BlazorPlayground.Graphics.Tests;

public class ShapeTests {
    [Fact]
    public void Definition() {
        var shape = new Line(new Point(100, 150), new Point(200, 250));

        Assert.Equal(ShapeDefinition.Get(typeof(Line)), shape.Definition);
    }

    [Fact]
    public void Transform() {
        var shape = new Line(new Point(100, 150), new Point(200, 250));

        shape.Transform(new Point(10, 20), false, 50, false, [new Point(50, 50)]);

        PointAssert.Equal(new Point(110, 170), shape.StartPoint);
        PointAssert.Equal(new Point(210, 270), shape.EndPoint);
    }

    [Fact]
    public void Transform_With_SnapToGrid() {
        var shape = new Line(new Point(105, 205), new Point(155, 255));

        shape.Transform(new Point(50, 50), true, 50, false, [new Point(151, 251)]);

        PointAssert.Equal(new Point(150, 250), shape.StartPoint);
        PointAssert.Equal(new Point(200, 300), shape.EndPoint);
    }

    [Fact]
    public void Transform_With_SnapToShapes() {
        var shape = new Line(new Point(105, 205), new Point(155, 255));

        shape.Transform(new Point(50, 50), false, 50, true, [new Point(151, 251)]);

        PointAssert.Equal(new Point(151, 251), shape.StartPoint);
        PointAssert.Equal(new Point(201, 301), shape.EndPoint);
    }

    [Fact]
    public void Transform_With_SnapToGrid_And_SnapToShapes_Grid() {
        var shape = new Line(new Point(105, 205), new Point(155, 255));

        shape.Transform(new Point(50, 50), true, 50, true, [new Point(149, 249)]);

        PointAssert.Equal(new Point(150, 250), shape.StartPoint);
        PointAssert.Equal(new Point(200, 300), shape.EndPoint);
    }

    [Fact]
    public void Transform_With_SnapToGrid_And_SnapToShapes_Points() {
        var shape = new Line(new Point(105, 205), new Point(155, 255));

        shape.Transform(new Point(50, 50), true, 50, true, [new Point(151, 251)]);

        PointAssert.Equal(new Point(151, 251), shape.StartPoint);
        PointAssert.Equal(new Point(201, 301), shape.EndPoint);
    }

    [Fact]
    public void Clone() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250));

        var result = polygon.Clone();

        Assert.NotSame(polygon, result);
    }

    [Fact]
    public void Clone_Opacity() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
            Opacity = 50
        };

        var result = polygon.Clone();

        Assert.Equal(50, Assert.IsType<IShapeWithOpacity>(result, false).Opacity);
    }

    [Fact]
    public void Clone_Fill() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
            Fill = new Color(255, 0, 255, 1)
        };

        var result = polygon.Clone();

        PaintServerAssert.Equal(new Color(255, 0, 255, 1), Assert.IsType<IShapeWithFill>(result, false).Fill);
    }

    [Fact]
    public void Clone_FillOpacity() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
            FillOpacity = 50
        };

        var result = polygon.Clone();

        Assert.Equal(50, Assert.IsType<IShapeWithFill>(result, false).FillOpacity);
    }

    [Fact]
    public void Clone_Stroke() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
            Stroke = new Color(255, 0, 255, 1)
        };

        var result = polygon.Clone();

        PaintServerAssert.Equal(new Color(255, 0, 255, 1), Assert.IsType<IShapeWithStroke>(result, false).Stroke);
    }

    [Fact]
    public void Clone_StrokeWidth() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
            StrokeWidth = 10
        };

        var result = polygon.Clone();

        Assert.Equal(10, Assert.IsType<IShapeWithStroke>(result, false).StrokeWidth);
    }

    [Fact]
    public void Clone_StrokeOpacity() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
            StrokeOpacity = 50
        };

        var result = polygon.Clone();

        Assert.Equal(50, Assert.IsType<IShapeWithStroke>(result, false).StrokeOpacity);
    }

    [Fact]
    public void Clone_StrokeLinecap() {
        var line = new Line(new Point(100, 150), new Point(200, 250)) {
            StrokeLinecap = Linecap.Square
        };

        var result = line.Clone();

        Assert.Equal(Linecap.Square, Assert.IsType<IShapeWithStrokeLinecap>(result, false).StrokeLinecap);
    }

    [Fact]
    public void Clone_StrokeLinejoin() {
        var rectangle = new Rectangle(new Point(100, 150), new Point(200, 250)) {
            StrokeLinejoin = Linejoin.Arcs
        };

        var result = rectangle.Clone();

        Assert.Equal(Linejoin.Arcs, Assert.IsType<IShapeWithStrokeLinejoin>(result, false).StrokeLinejoin);
    }

    [Fact]
    public void Clone_Sides() {
        var polygon = new RegularPolygon(new Point(100, 150), new Point(200, 250)) {
            Sides = 5
        };

        var result = polygon.Clone();

        Assert.Equal(5, Assert.IsType<IShapeWithSides>(result, false).Sides);
    }
}
