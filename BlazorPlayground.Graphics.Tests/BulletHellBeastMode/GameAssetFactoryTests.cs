using BlazorPlayground.Graphics.BulletHellBeastMode;
using NetTopologySuite.Geometries;
using NSubstitute;
using System.Collections.Generic;
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

        rectangle.Fill = new Color(255, 0, 0, 1);
        rectangle.FillOpacity = 50;
        rectangle.SetStroke(new Color(0, 0, 255, 1));
        rectangle.SetStrokeOpacity(80);
        rectangle.SetStrokeWidth(2);
        rectangle.Opacity = 90;

        var result = subject.GetGameAsset([rectangle]);

        Assert.NotNull(result);

        var section = Assert.Single(result.Sections);

        Assert.Equal(geometryFactory.CreatePolygon([new(-30, 10), new(-30, -10), new(30, -10), new(30, 10), new(-30, 10)]), section.Geometry);
        Assert.Equal("rgba(255, 0, 0, 0.5)", section.FillColor);
        Assert.Equal("rgba(0, 0, 255, 0.8)", section.StrokeColor);
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

        Assert.Equal(GameAssetFactory.DefaultColor, section.FillColor);
        Assert.Equal(GameAssetFactory.DefaultColor, section.StrokeColor);
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
        var result = GameAssetFactory.GetOrigin([]);

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

    [Fact]
    public void GetGameElementPathFromEmptyEnumerable() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new GameAssetFactory(geometryFactory);

        var sections = Enumerable.Empty<DrawableShape>();

        var result = subject.GetGameElementPath(new(10, 20), sections);

        Assert.Empty(result.Coordinates);
    }
    
    [Fact]
    public void GetGameElementPathFromSingleDrawableShape() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new GameAssetFactory(geometryFactory);

        var sections = new List<DrawableShape>() {
            new Line(new(2, 3), new(12, 13))
        };

        var result = subject.GetGameElementPath(new(10, 20), sections);

        var expected = new List<Coordinate>() {
            new(-8, -17),
            new(2, -7)
        };

        Assert.Equal(expected.Count, result.Coordinates.Count);
        for (int i = 0; i < expected.Count; i++) {
            Assert.Equal(expected[i].X, result.Coordinates[i].X);
            Assert.Equal(expected[i].Y, result.Coordinates[i].Y);
        }
    }

    [Fact]
    public void GetGameElementPath() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(10));
        var subject = new GameAssetFactory(geometryFactory);

        var sections = new List<DrawableShape>() {
            new Line(new(2, 3), new(12, 13)),
            new Line(new(25, 26), new(12, 13)),
            new Line(new(0, 0), new(-9, -8)),
            new Rectangle(new(100, 200), new(300, 400)),
            new Line(new(26, 26), new(30, 30)),
            new Line(new(-20, -20), new(-9, -9))
        };

        var result = subject.GetGameElementPath(new(10, 20), sections);

        var expected = new List<Coordinate>() {
            new(-30, -40),
            new(-19, -29),
            new(-19, -28),
            new(-10, -20),
            new(-8, -17),
            new(2, -7),
            new(15, 6),
            new(16, 6),
            new(20, 10)
        };

        Assert.Equal(expected.Count, result.Coordinates.Count);
        for (int i = 0; i < expected.Count; i++) {
            Assert.Equal(expected[i].X, result.Coordinates[i].X);
            Assert.Equal(expected[i].Y, result.Coordinates[i].Y);
        }
    }
}
