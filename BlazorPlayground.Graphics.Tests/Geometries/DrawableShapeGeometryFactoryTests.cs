using BlazorPlayground.Graphics.Geometries;
using NetTopologySuite.Geometries;
using Xunit;

namespace BlazorPlayground.Graphics.Tests.Geometries;

public class DrawableShapeGeometryFactoryTests {
    [Fact]
    public void GetGeometryFromRectangle() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var rectangle = new Rectangle(new(-10, -10), new(50, -30));

        var result = subject.GetGeometry(rectangle);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(-10, -10), new(-10, -30), new(50, -30), new(50, -10), new(-10, -10)]), result);
    }

    [Fact]
    public void GetGeometryFromRegularPolygon() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var regularPolygon = new RegularPolygon(new(50, 50), new(50, 100));

        regularPolygon.SetSides(3);

        var result = subject.GetGeometry(regularPolygon);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(50, 100), new(6.699, 25), new(93.301, 25), new(50, 100)]), result);
    }

    [Fact]
    public void GetGeometryFromCircle() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var circle = new Circle(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(circle);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(83.852, 50), new(83.557, 55.629), new(82.675, 61.196), new(81.216, 66.641), new(79.196, 71.903), new(76.637, 76.926), new(73.567, 81.653), new(70.02, 86.034), new(66.034, 90.02), new(61.653, 93.567), new(56.926, 96.637), new(51.903, 99.196), new(46.641, 101.216), new(41.196, 102.675), new(35.629, 103.557), new(30, 103.852), new(24.371, 103.557), new(18.804, 102.675), new(13.359, 101.216), new(8.097, 99.196), new(3.074, 96.637), new(-1.653, 93.567), new(-6.034, 90.02), new(-10.02, 86.034), new(-13.567, 81.653), new(-16.637, 76.926), new(-19.196, 71.903), new(-21.216, 66.641), new(-22.675, 61.196), new(-23.557, 55.629), new(-23.852, 50), new(-23.557, 44.371), new(-22.675, 38.804), new(-21.216, 33.359), new(-19.196, 28.097), new(-16.637, 23.074), new(-13.567, 18.347), new(-10.02, 13.966), new(-6.034, 9.98), new(-1.653, 6.433), new(3.074, 3.363), new(8.097, 0.804), new(13.359, -1.216), new(18.804, -2.675), new(24.371, -3.557), new(30, -3.852), new(35.629, -3.557), new(41.196, -2.675), new(46.641, -1.216), new(51.903, 0.804), new(56.926, 3.363), new(61.653, 6.433), new(66.034, 9.98), new(70.02, 13.966), new(73.567, 18.347), new(76.637, 23.074), new(79.196, 28.097), new(81.216, 33.359), new(82.675, 38.804), new(83.557, 44.371), new(83.852, 50)]), result);
    }

    [Fact]
    public void GetGeometryFromEllipse() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var ellipse = new Ellipse(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(ellipse);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(50, 50), new(49.89, 55.226), new(49.563, 60.396), new(49.021, 65.451), new(48.271, 70.337), new(47.321, 75), new(46.18, 79.389), new(44.863, 83.457), new(43.383, 87.157), new(41.756, 90.451), new(40, 93.301), new(38.135, 95.677), new(36.18, 97.553), new(34.158, 98.907), new(32.091, 99.726), new(30, 100), new(27.909, 99.726), new(25.842, 98.907), new(23.82, 97.553), new(21.865, 95.677), new(20, 93.301), new(18.244, 90.451), new(16.617, 87.157), new(15.137, 83.457), new(13.82, 79.389), new(12.679, 75), new(11.729, 70.337), new(10.979, 65.451), new(10.437, 60.396), new(10.11, 55.226), new(10, 50), new(10.11, 44.774), new(10.437, 39.604), new(10.979, 34.549), new(11.729, 29.663), new(12.679, 25), new(13.82, 20.611), new(15.137, 16.543), new(16.617, 12.843), new(18.244, 9.549), new(20, 6.699), new(21.865, 4.323), new(23.82, 2.447), new(25.842, 1.093), new(27.909, 0.274), new(30, 0), new(32.091, 0.274), new(34.158, 1.093), new(36.18, 2.447), new(38.135, 4.323), new(40, 6.699), new(41.756, 9.549), new(43.383, 12.843), new(44.863, 16.543), new(46.18, 20.611), new(47.321, 25), new(48.271, 29.663), new(49.021, 34.549), new(49.563, 39.604), new(49.89, 44.774), new(50, 50)]), result);
    }

    [Fact]
    public void GetGeometryFromLine() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var line = new Line(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(line);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(30, 50), new(50, 100)]), result);
    }

    [Fact]
    public void GetGeometryFromQuadraticBezier() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var quadraticBezier = new QuadraticBezier(new(30, 50), new(50, 100)) {
            ControlPoint = new(20, 70)
        };

        var result = subject.GetGeometry(quadraticBezier);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(30, 50), new(29.678, 50.669), new(29.378, 51.344), new(29.1, 52.025), new(28.844, 52.711), new(28.611, 53.403), new(28.4, 54.1), new(28.211, 54.803), new(28.044, 55.511), new(27.9, 56.225), new(27.778, 56.944), new(27.678, 57.669), new(27.6, 58.4), new(27.544, 59.136), new(27.511, 59.878), new(27.5, 60.625), new(27.511, 61.378), new(27.544, 62.136), new(27.6, 62.9), new(27.678, 63.669), new(27.778, 64.444), new(27.9, 65.225), new(28.044, 66.011), new(28.211, 66.803), new(28.4, 67.6), new(28.611, 68.403), new(28.844, 69.211), new(29.1, 70.025), new(29.378, 70.844), new(29.678, 71.669), new(30, 72.5), new(30.344, 73.336), new(30.711, 74.178), new(31.1, 75.025), new(31.511, 75.878), new(31.944, 76.736), new(32.4, 77.6), new(32.878, 78.469), new(33.378, 79.344), new(33.9, 80.225), new(34.444, 81.111), new(35.011, 82.003), new(35.6, 82.9), new(36.211, 83.803), new(36.844, 84.711), new(37.5, 85.625), new(38.178, 86.544), new(38.878, 87.469), new(39.6, 88.4), new(40.344, 89.336), new(41.111, 90.278), new(41.9, 91.225), new(42.711, 92.178), new(43.544, 93.136), new(44.4, 94.1), new(45.278, 95.069), new(46.178, 96.044), new(47.1, 97.025), new(48.044, 98.011), new(49.011, 99.003), new(50, 100)]), result);
    }

    [Fact]
    public void GetGeometryFromCubicBezier() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var quadraticBezier = new CubicBezier(new(30, 50), new(50, 100)) {
            ControlPoint1 = new(20, 60),
            ControlPoint2 = new(10, 90)
        };

        var result = subject.GetGeometry(quadraticBezier);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(30, 50), new(29.5, 50.516), new(29.002, 51.065), new(28.506, 51.645), new(28.015, 52.255), new(27.529, 52.894), new(27.05, 53.56), new(26.579, 54.253), new(26.119, 54.972), new(25.669, 55.715), new(25.231, 56.481), new(24.808, 57.27), new(24.4, 58.08), new(24.009, 58.91), new(23.635, 59.759), new(23.281, 60.625), new(22.948, 61.508), new(22.637, 62.407), new(22.35, 63.32), new(22.088, 64.246), new(21.852, 65.185), new(21.644, 66.135), new(21.465, 67.095), new(21.316, 68.064), new(21.2, 69.04), new(21.117, 70.023), new(21.069, 71.012), new(21.056, 72.005), new(21.081, 73.001), new(21.146, 74), new(21.25, 75), new(21.396, 76), new(21.585, 76.999), new(21.819, 77.995), new(22.098, 78.988), new(22.425, 79.977), new(22.8, 80.96), new(23.225, 81.936), new(23.702, 82.905), new(24.231, 83.865), new(24.815, 84.815), new(25.454, 85.754), new(26.15, 86.68), new(26.904, 87.593), new(27.719, 88.492), new(28.594, 89.375), new(29.531, 90.241), new(30.533, 91.09), new(31.6, 91.92), new(32.734, 92.73), new(33.935, 93.519), new(35.206, 94.285), new(36.548, 95.028), new(37.962, 95.747), new(39.45, 96.44), new(41.013, 97.106), new(42.652, 97.745), new(44.369, 98.355), new(46.165, 98.935), new(48.041, 99.484), new(50, 100)]), result);
    }

    [Fact]
    public void GetGeometryFromClosedPathWithTwoCoordinates() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var closedPath = new ClosedPath(new(30, 50), new(50, 100));

        var result = subject.GetGeometry(closedPath);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreateLineString([new(30, 50), new(50, 100)]), result);
    }

    [Fact]
    public void GetGeometryFromClosedPath() {
        var geometryFactory = new GeometryFactory(new PrecisionModel(1000));
        var subject = new DrawableShapeGeometryFactory(geometryFactory);
        var closedPath = new ClosedPath(new(30, 50), new(50, 100)) {
            IntermediatePoints = {
                new(60, 40),
                new(30, 10)
            }
        };

        var result = subject.GetGeometry(closedPath);

        Assert.NotNull(result);
        Assert.Equal(geometryFactory.CreatePolygon([new(30, 50), new(50, 100), new(60, 40), new(30, 10), new(30, 50)]), result);
    }
}
