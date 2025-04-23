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

        Assert.IsType<Polygon>(section.Geometry);
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
    public void GetOriginFromRectangle() {
        var result = GameElementFactory.GetOrigin([new Rectangle(new(120, 80), new(-10, -30))]);

        Assert.NotNull(result);
        PointAssert.Equal(new(55, 25), result);
    }

    [Fact]
    public void GetOriginFromRegularPolygon() {
        var result = GameElementFactory.GetOrigin([new RegularPolygon(new(50, 50), new(50, 100))]);

        Assert.NotNull(result);
        PointAssert.Equal(new(50, 50), result);
    }

    [Fact]
    public void GetOriginFromCircle() {
        var result = GameElementFactory.GetOrigin([new Circle(new(30, 50), new(50, 100))]);

        Assert.NotNull(result);
        PointAssert.Equal(new(30, 50), result);
    }

    [Fact]
    public void GetOriginFromEllipse() {
        var result = GameElementFactory.GetOrigin([new Ellipse(new(30, 50), new(50, 100))]);

        Assert.NotNull(result);
        PointAssert.Equal(new(30, 50), result);
    }

    [Fact]
    public void GetGeometryFromRectangle() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var rectangle = new Rectangle(new(-10, -10), new(50, -30));

        var result = subject.GetGeometry(rectangle, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(90, 90), new(90, 70), new(150, 70), new(150, 90), new(90, 90)]), result);
    }

    [Fact]
    public void GetGeometryFromRegularPolygon() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var regularPolygon = new RegularPolygon(new(50, 50), new(50, 100));

        regularPolygon.SetSides(3);

        var result = subject.GetGeometry(regularPolygon, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(150, 200), new(106.699, 125), new(193.301, 125), new(150, 200)]), result);
    }

    [Fact]
    public void GetGeometryFromCircle() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var circle = new Circle(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(circle, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(183.852, 150), new(183.557, 155.629), new(182.675, 161.196), new(181.216, 166.641), new(179.196, 171.903), new(176.637, 176.926), new(173.567, 181.653), new(170.02, 186.034), new(166.034, 190.02), new(161.653, 193.567), new(156.926, 196.637), new(151.903, 199.196), new(146.641, 201.216), new(141.196, 202.675), new(135.629, 203.557), new(130, 203.852), new(124.371, 203.557), new(118.804, 202.675), new(113.359, 201.216), new(108.097, 199.196), new(103.074, 196.637), new(98.347, 193.567), new(93.966, 190.02), new(89.98, 186.034), new(86.433, 181.653), new(83.363, 176.926), new(80.804, 171.903), new(78.784, 166.641), new(77.325, 161.196), new(76.443, 155.629), new(76.148, 150), new(76.443, 144.371), new(77.325, 138.804), new(78.784, 133.359), new(80.804, 128.097), new(83.363, 123.074), new(86.433, 118.347), new(89.98, 113.966), new(93.966, 109.98), new(98.347, 106.433), new(103.074, 103.363), new(108.097, 100.804), new(113.359, 98.784), new(118.804, 97.325), new(124.371, 96.443), new(130, 96.148), new(135.629, 96.443), new(141.196, 97.325), new(146.641, 98.784), new(151.903, 100.804), new(156.926, 103.363), new(161.653, 106.433), new(166.034, 109.98), new(170.02, 113.966), new(173.567, 118.347), new(176.637, 123.074), new(179.196, 128.097), new(181.216, 133.359), new(182.675, 138.804), new(183.557, 144.371), new(183.852, 150)]), result);
    }

    [Fact]
    public void GetGeometryFromEllipse() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var ellipse = new Ellipse(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(ellipse, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(150, 150), new(149.89, 155.226), new(149.563, 160.396), new(149.021, 165.451), new(148.271, 170.337), new(147.321, 175), new(146.18, 179.389), new(144.863, 183.457), new(143.383, 187.157), new(141.756, 190.451), new(140, 193.301), new(138.135, 195.677), new(136.18, 197.553), new(134.158, 198.907), new(132.091, 199.726), new(130, 200), new(127.909, 199.726), new(125.842, 198.907), new(123.82, 197.553), new(121.865, 195.677), new(120, 193.301), new(118.244, 190.451), new(116.617, 187.157), new(115.137, 183.457), new(113.82, 179.389), new(112.679, 175), new(111.729, 170.337), new(110.979, 165.451), new(110.437, 160.396), new(110.11, 155.226), new(110, 150), new(110.11, 144.774), new(110.437, 139.604), new(110.979, 134.549), new(111.729, 129.663), new(112.679, 125), new(113.82, 120.611), new(115.137, 116.543), new(116.617, 112.843), new(118.244, 109.549), new(120, 106.699), new(121.865, 104.323), new(123.82, 102.447), new(125.842, 101.093), new(127.909, 100.274), new(130, 100), new(132.091, 100.274), new(134.158, 101.093), new(136.18, 102.447), new(138.135, 104.323), new(140, 106.699), new(141.756, 109.549), new(143.383, 112.843), new(144.863, 116.543), new(146.18, 120.611), new(147.321, 125), new(148.271, 129.663), new(149.021, 134.549), new(149.563, 139.604), new(149.89, 144.774), new(150, 150)]), result);
    }

    [Fact]
    public void GetGeometryFromLine() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var line = new Line(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(line, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(130, 150), new(150, 200)]), result);
    }

    [Fact]
    public void GetGeometryFromQuadraticBezier() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var quadraticBezier = new QuadraticBezier(new(30, 50), new(50, 100)) {
            ControlPoint = new(20, 70)
        };

        var result = subject.GetGeometry(quadraticBezier, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(130, 150), new(129.678, 150.669), new(129.378, 151.344), new(129.1, 152.025), new(128.844, 152.711), new(128.611, 153.403), new(128.4, 154.1), new(128.211, 154.803), new(128.044, 155.511), new(127.9, 156.225), new(127.778, 156.944), new(127.678, 157.669), new(127.6, 158.4), new(127.544, 159.136), new(127.511, 159.878), new(127.5, 160.625), new(127.511, 161.378), new(127.544, 162.136), new(127.6, 162.9), new(127.678, 163.669), new(127.778, 164.444), new(127.9, 165.225), new(128.044, 166.011), new(128.211, 166.803), new(128.4, 167.6), new(128.611, 168.403), new(128.844, 169.211), new(129.1, 170.025), new(129.378, 170.844), new(129.678, 171.669), new(130, 172.5), new(130.344, 173.336), new(130.711, 174.178), new(131.1, 175.025), new(131.511, 175.878), new(131.944, 176.736), new(132.4, 177.6), new(132.878, 178.469), new(133.378, 179.344), new(133.9, 180.225), new(134.444, 181.111), new(135.011, 182.003), new(135.6, 182.9), new(136.211, 183.803), new(136.844, 184.711), new(137.5, 185.625), new(138.178, 186.544), new(138.878, 187.469), new(139.6, 188.4), new(140.344, 189.336), new(141.111, 190.278), new(141.9, 191.225), new(142.711, 192.178), new(143.544, 193.136), new(144.4, 194.1), new(145.278, 195.069), new(146.178, 196.044), new(147.1, 197.025), new(148.044, 198.011), new(149.011, 199.003), new(150, 200)]), result);
    }

    [Fact]
    public void GetGeometryFromCubicBezier() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var quadraticBezier = new CubicBezier(new(30, 50), new(50, 100)) {
            ControlPoint1 = new(20, 60),
            ControlPoint2 = new(10, 90)
        };

        var result = subject.GetGeometry(quadraticBezier, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(130, 150), new(129.5, 150.516), new(129.002, 151.065), new(128.506, 151.645), new(128.015, 152.255), new(127.529, 152.894), new(127.05, 153.56), new(126.579, 154.253), new(126.119, 154.972), new(125.669, 155.715), new(125.231, 156.481), new(124.808, 157.27), new(124.4, 158.08), new(124.009, 158.91), new(123.635, 159.759), new(123.281, 160.625), new(122.948, 161.508), new(122.637, 162.407), new(122.35, 163.32), new(122.088, 164.246), new(121.852, 165.185), new(121.644, 166.135), new(121.465, 167.095), new(121.316, 168.064), new(121.2, 169.04), new(121.117, 170.023), new(121.069, 171.012), new(121.056, 172.005), new(121.081, 173.001), new(121.146, 174), new(121.25, 175), new(121.396, 176), new(121.585, 176.999), new(121.819, 177.995), new(122.098, 178.988), new(122.425, 179.977), new(122.8, 180.96), new(123.225, 181.936), new(123.702, 182.905), new(124.231, 183.865), new(124.815, 184.815), new(125.454, 185.754), new(126.15, 186.68), new(126.904, 187.593), new(127.719, 188.492), new(128.594, 189.375), new(129.531, 190.241), new(130.533, 191.09), new(131.6, 191.92), new(132.734, 192.73), new(133.935, 193.519), new(135.206, 194.285), new(136.548, 195.028), new(137.962, 195.747), new(139.45, 196.44), new(141.013, 197.106), new(142.652, 197.745), new(144.369, 198.355), new(146.165, 198.935), new(148.041, 199.484), new(150, 200)]), result);
    }

    [Fact]
    public void GetGeometryFromClosedPathWithTwoCoordinates() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var closedPath = new ClosedPath(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(closedPath, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(130, 150), new(150, 200)]), result);
    }

    [Fact]
    public void GetGeometryFromClosedPath() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new GameElementFactory(geometryFactory);
        var closedPath = new ClosedPath(new(30, 50), new(50, 100)) {
            IntermediatePoints = {
                new(60, 40),
                new(30, 10)
            }
        };

        var result = subject.GetGeometry(closedPath, new(-100, -100));

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(130, 150), new(150, 200), new(160, 140), new(130, 110), new(130, 150)]), result);
    }

    [Fact]
    public void GetFillColorFromShapeWithoutFill() {
        var shape = Substitute.For<DrawableShape>();

        var result = GameElementFactory.GetFillColor(shape);

        Assert.Equal(GameElementFactory.DefaultColor, result);
    }

    [Fact]
    public void GetFillColorFromShapeWithFill() {
        var rectangle = new Rectangle(new(-10, -10), new(50, -30));

        rectangle.SetFill(new Color(255, 128, 64, 0.5));
        rectangle.SetFillOpacity(80);

        var result = GameElementFactory.GetFillColor(rectangle);

        Assert.NotNull(result);
        Assert.Equal(255, result.Red);
        Assert.Equal(128, result.Green);
        Assert.Equal(64, result.Blue);
        Assert.Equal(0.4, result.Alpha);
    }

    [Fact]
    public void GetStrokeColorFromShapeWithoutStroke() {
        var shape = Substitute.For<DrawableShape>();

        var result = GameElementFactory.GetStrokeColor(shape);

        Assert.Equal(GameElementFactory.DefaultColor, result);
    }

    [Fact]
    public void GetStrokeColorFromShapeWithStroke() {
        var rectangle = new Rectangle(new(-10, -10), new(50, -30));

        rectangle.SetStroke(new Color(255, 128, 64, 0.5));
        rectangle.SetStrokeOpacity(80);

        var result = GameElementFactory.GetStrokeColor(rectangle);

        Assert.NotNull(result);
        Assert.Equal(255, result.Red);
        Assert.Equal(128, result.Green);
        Assert.Equal(64, result.Blue);
        Assert.Equal(0.4, result.Alpha);
    }

    [Fact]
    public void GetStrokeWidthFromShapeWithoutStroke() {
        var shape = Substitute.For<DrawableShape>();

        var result = GameElementFactory.GetStrokeWidth(shape);

        Assert.Equal(GameElementFactory.DefaultStrokeWidth, result);
    }

    [Fact]
    public void GetStrokeWidthFromShapeWithStroke() {
        var rectangle = new Rectangle(new(-10, -10), new(50, -30));

        rectangle.SetStrokeWidth(5);

        var result = GameElementFactory.GetStrokeWidth(rectangle);

        Assert.Equal(5, result);
    }

    [Fact]
    public void GetOpacityFromShapeWithoutOpacity() {
        var shape = Substitute.For<DrawableShape>();

        var result = GameElementFactory.GetOpacity(shape);

        Assert.Equal(GameElementFactory.DefaultOpacity, result);
    }

    [Fact]
    public void GetOpacityFromShapeWithOpacity() {
        var rectangle = new Rectangle(new(-10, -10), new(50, -30));

        rectangle.SetOpacity(50);

        var result = GameElementFactory.GetOpacity(rectangle);

        Assert.Equal(0.5, result);
    }
}
