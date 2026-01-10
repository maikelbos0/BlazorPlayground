using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace BlazorPlayground.Graphics;

public class CubicBezier : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithStrokeLinecap, IAutoSelectedShape {
    private readonly static Anchor[] anchors = [
        new Anchor<CubicBezier>(s => s.StartPoint, (s, p) => s.StartPoint = p),
        new Anchor<CubicBezier>(s => s.ControlPoint1, (s, p) => s.ControlPoint1 = p),
        new Anchor<CubicBezier>(s => s.ControlPoint2, (s, p) => s.ControlPoint2 = p),
        new Anchor<CubicBezier>(s => s.EndPoint, (s, p) => s.EndPoint = p)
    ];

    public override string ElementName => "path";
    public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
    public Point StartPoint { get; set; }
    public Point ControlPoint1 { get; set; }
    public Point ControlPoint2 { get; set; }
    public Point EndPoint { get; set; }

    private CubicBezier() : this(new Point(0, 0), new Point(0, 0)) { }

    public CubicBezier(Point startPoint, Point endPoint) {
        StartPoint = startPoint;
        EndPoint = endPoint;

        var step = (EndPoint - startPoint) / 3;

        ControlPoint1 = StartPoint + step;
        ControlPoint2 = StartPoint + step * 2;
    }

    public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly([StartPoint, EndPoint]);

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", FormattableString.Invariant($"M {StartPoint.X} {StartPoint.Y} C {ControlPoint1.X} {ControlPoint1.Y}, {ControlPoint2.X} {ControlPoint2.Y}, {EndPoint.X} {EndPoint.Y}") }
    };

    protected override Shape CreateClone() => new CubicBezier(StartPoint, EndPoint) {
        ControlPoint1 = ControlPoint1,
        ControlPoint2 = ControlPoint2
    };

    public override Geometry GetGeometry(GeometryFactory geometryFactory, Point origin) {
        var coordinates = new Coordinate[CurveApproximationSegmentCount + 1];

        for (var i = 1; i < CurveApproximationSegmentCount; i++) {
            var step = CurveAproximationStepIncrement * i;
            var invertedStep = 1.0 - step;
            var intermediatePoint = StartPoint * invertedStep * invertedStep * invertedStep
                + ControlPoint1 * step * invertedStep * invertedStep * 3
                + ControlPoint2 * step * step * invertedStep * 3
                + EndPoint * step * step * step;

            coordinates[i] = geometryFactory.GetCoordinate(intermediatePoint.X, intermediatePoint.Y, origin);
        }

        coordinates[0] = geometryFactory.GetCoordinate(StartPoint.X, StartPoint.Y, origin);
        coordinates[^1] = geometryFactory.GetCoordinate(EndPoint.X, EndPoint.Y, origin);

        return geometryFactory.CreateLineString(coordinates);
    }

    public override BoundingBox GetBoundingBox()
        => new(
            new(Math.Min(StartPoint.X, EndPoint.X), Math.Min(StartPoint.Y, EndPoint.Y)),
            new(Math.Max(StartPoint.X, EndPoint.X), Math.Max(StartPoint.Y, EndPoint.Y))
        );
}
