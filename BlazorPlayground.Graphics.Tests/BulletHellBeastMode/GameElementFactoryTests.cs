using BlazorPlayground.Graphics.BulletHellBeastMode;
using NetTopologySuite.Geometries;
using NSubstitute;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests.BulletHellBeastMode;

public class GameElementFactoryTests {
    [Fact]
    public void GetGameElementFromEmptyEnumerable() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);

        var result = subject.GetGameElement([]);

        Assert.NotNull(result);
        Assert.Empty(result.Sections);
    }

    [Fact]
    public void GetGameElementFromSingleDrawableShape() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var rectangle = new Rectangle(new(-10, -10), new(50, -30));

        rectangle.SetFill(new Color(255, 0, 0, 1));
        rectangle.SetFillOpacity(50);
        rectangle.SetStroke(new Color(0, 0, 255, 1));
        rectangle.SetStrokeOpacity(80);
        rectangle.SetStrokeWidth(2);
        rectangle.SetOpacity(90);

        var result = subject.GetGameElement([rectangle]);

        Assert.NotNull(result);

        var section = Assert.Single(result.Sections);

        Assert.Equal(geometryFactory.CreatePolygon([new(-30, 10), new(-30, -10), new(30, -10), new(30, 10), new(-30, 10)]), section.Geometry);
        Assert.Equal(255, section.FillColor.Red);
        Assert.Equal(0, section.FillColor.Green);
        Assert.Equal(0, section.FillColor.Blue);
        Assert.Equal(0.5, section.FillColor.Alpha);
        Assert.Equal(0, section.StrokeColor.Red);
        Assert.Equal(0, section.StrokeColor.Green);
        Assert.Equal(255, section.StrokeColor.Blue);
        Assert.Equal(0.8, section.StrokeColor.Alpha);
        Assert.Equal(2, section.StrokeWidth);
        Assert.Equal(0.9, section.Opacity);
    }

    [Fact]
    public void GetGameElementFromSingleDrawableShapeWithoutProperties() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var shape = Substitute.For<DrawableShape>();
        shape.GetGeometry(geometryFactory, Arg.Any<Point>()).Returns(Polygon.Empty);
        shape.GetBoundingBox().Returns(new BoundingBox(0, 0, 0, 0));

        var result = subject.GetGameElement([shape]);

        Assert.NotNull(result);

        var section = Assert.Single(result.Sections);

        Assert.Equal(GameElementFactory.DefaultColor.Red, section.FillColor.Red);
        Assert.Equal(GameElementFactory.DefaultColor.Green, section.FillColor.Green);
        Assert.Equal(GameElementFactory.DefaultColor.Blue, section.FillColor.Blue);
        Assert.Equal(GameElementFactory.DefaultColor.Alpha, section.FillColor.Alpha);
        Assert.Equal(GameElementFactory.DefaultColor.Red, section.StrokeColor.Red);
        Assert.Equal(GameElementFactory.DefaultColor.Green, section.StrokeColor.Green);
        Assert.Equal(GameElementFactory.DefaultColor.Blue, section.StrokeColor.Blue);
        Assert.Equal(GameElementFactory.DefaultColor.Alpha, section.StrokeColor.Alpha);
        Assert.Equal(GameElementFactory.DefaultStrokeWidth, section.StrokeWidth);
        Assert.Equal(GameElementFactory.DefaultOpacity, section.Opacity);
    }

    [Fact]
    public void GetGameElementFromMultipleDrawableShapes() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);

        var result = subject.GetGameElement([
            new Rectangle(new(-10, -10), new(50, -30)),
            new Line(new(20, 30), new(40, 50))
        ]);

        Assert.NotNull(result);

        Assert.Equal(2, result.Sections.Count);
    }

    [Fact]
    public void GetOriginFromEmptyEnumerable() {
        var result = GameElementFactory.GetOrigin(Enumerable.Empty<DrawableShape>());

        Assert.NotNull(result);
        PointAssert.Equal(new(0, 0), result);
    }

    [Fact]
    public void GetOriginFromSingleDrawableShape() {
        var result = GameElementFactory.GetOrigin([new Line(new(30, 50), new(50, 100))]);

        Assert.NotNull(result);
        PointAssert.Equal(new(40, 75), result);
    }

    [Fact]
    public void GetOriginFromMultipleDrawableShapesShapes() {
        var result = GameElementFactory.GetOrigin([
            new Rectangle(new(-10, -10), new(40, 40)),
            new Line(new(30, 50), new(50, 100))
        ]);

        Assert.NotNull(result);
        PointAssert.Equal(new(20, 45), result);
    }
}
