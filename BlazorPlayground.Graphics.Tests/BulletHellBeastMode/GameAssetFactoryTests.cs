using BlazorPlayground.Graphics.BulletHellBeastMode;
using NetTopologySuite.Geometries;
using NSubstitute;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests.BulletHellBeastMode;

public class GameAssetFactoryTests {
    [Fact]
    public void GetGameAssetFromEmptyEnumerable() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new GameAssetFactory(geometryFactory);

        var result = subject.GetGameAsset([]);

        Assert.NotNull(result);
        Assert.Empty(result.Sections);
    }

    [Fact]
    public void GetGameAssetFromSingleDrawableShape() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new GameAssetFactory(geometryFactory);
        var rectangle = new Rectangle(new(-10, -10), new(50, -30));

        rectangle.SetFill(new Color(255, 0, 0, 1));
        rectangle.SetFillOpacity(50);
        rectangle.SetStroke(new Color(0, 0, 255, 1));
        rectangle.SetStrokeOpacity(80);
        rectangle.SetStrokeWidth(2);
        rectangle.SetOpacity(90);

        var result = subject.GetGameAsset([rectangle]);

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
    public void GetGameAssetFromSingleDrawableShapeWithoutProperties() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new GameAssetFactory(geometryFactory);
        var shape = Substitute.For<DrawableShape>();
        shape.GetGeometry(geometryFactory, Arg.Any<Point>()).Returns(Polygon.Empty);
        shape.GetBoundingBox().Returns(new BoundingBox(0, 0, 0, 0));

        var result = subject.GetGameAsset([shape]);

        Assert.NotNull(result);

        var section = Assert.Single(result.Sections);

        Assert.Equal(GameAssetFactory.DefaultColor.Red, section.FillColor.Red);
        Assert.Equal(GameAssetFactory.DefaultColor.Green, section.FillColor.Green);
        Assert.Equal(GameAssetFactory.DefaultColor.Blue, section.FillColor.Blue);
        Assert.Equal(GameAssetFactory.DefaultColor.Alpha, section.FillColor.Alpha);
        Assert.Equal(GameAssetFactory.DefaultColor.Red, section.StrokeColor.Red);
        Assert.Equal(GameAssetFactory.DefaultColor.Green, section.StrokeColor.Green);
        Assert.Equal(GameAssetFactory.DefaultColor.Blue, section.StrokeColor.Blue);
        Assert.Equal(GameAssetFactory.DefaultColor.Alpha, section.StrokeColor.Alpha);
        Assert.Equal(GameAssetFactory.DefaultStrokeWidth, section.StrokeWidth);
        Assert.Equal(GameAssetFactory.DefaultOpacity, section.Opacity);
    }

    [Fact]
    public void GetGameAssetFromMultipleDrawableShapes() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new GameAssetFactory(geometryFactory);

        var result = subject.GetGameAsset([
            new Rectangle(new(-10, -10), new(50, -30)),
            new Line(new(20, 30), new(40, 50))
        ]);

        Assert.NotNull(result);

        Assert.Equal(2, result.Sections.Count);
    }

    [Fact]
    public void GetOriginFromEmptyEnumerable() {
        var result = GameAssetFactory.GetOrigin(Enumerable.Empty<DrawableShape>());

        Assert.NotNull(result);
        PointAssert.Equal(new(0, 0), result);
    }

    [Fact]
    public void GetOriginFromSingleDrawableShape() {
        var result = GameAssetFactory.GetOrigin([new Line(new(30, 50), new(50, 100))]);

        Assert.NotNull(result);
        PointAssert.Equal(new(40, 75), result);
    }

    [Fact]
    public void GetOriginFromMultipleDrawableShapesShapes() {
        var result = GameAssetFactory.GetOrigin([
            new Rectangle(new(-10, -10), new(40, 40)),
            new Line(new(30, 50), new(50, 100))
        ]);

        Assert.NotNull(result);
        PointAssert.Equal(new(20, 45), result);
    }
}
