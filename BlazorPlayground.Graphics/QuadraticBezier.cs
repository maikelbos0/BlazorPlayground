using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace BlazorPlayground.Graphics;

public class QuadraticBezier : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithStrokeLinecap, IAutoSelectedShape {
    private readonly static Anchor[] anchors = [
        new Anchor<QuadraticBezier>(s => s.StartPoint, (s, p) => s.StartPoint = p),
        new Anchor<QuadraticBezier>(s => s.ControlPoint, (s, p) => s.ControlPoint = p),
        new Anchor<QuadraticBezier>(s => s.EndPoint, (s, p) => s.EndPoint = p)
    ];

    public override string ElementName => "path";
    public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
    public Point StartPoint { get; set; }
    public Point ControlPoint { get; set; }
    public Point EndPoint { get; set; }

    private QuadraticBezier() : this(new Point(0, 0), new Point(0, 0)) { }

    public QuadraticBezier(Point startPoint, Point endPoint) {
        StartPoint = startPoint;
        ControlPoint = (startPoint + endPoint) / 2;
        EndPoint = endPoint;
    }

    public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly([StartPoint, EndPoint]);

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", FormattableString.Invariant($"M {StartPoint.X} {StartPoint.Y} Q {ControlPoint.X} {ControlPoint.Y}, {EndPoint.X} {EndPoint.Y}") }
    };

    protected override Shape CreateClone() => new QuadraticBezier(StartPoint, EndPoint) {
        ControlPoint = ControlPoint
    };

    public override Geometry GetGeometry(GeometryFactory geometryFactory, Point origin) {
        var coordinates = new Coordinate[CurveApproximationSegmentCount + 1];

        for (var i = 1; i < CurveApproximationSegmentCount; i++) {
            var step = CurveAproximationStepIncrement * i;
            var invertedStep = 1.0 - step;
            var intermediatePoint = StartPoint * invertedStep * invertedStep
                + ControlPoint * step * invertedStep * 2
                + EndPoint * step * step;

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