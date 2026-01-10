using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace BlazorPlayground.Graphics;

public class Line : DrawableShape, IShapeWithOpacity, IShapeWithStroke, IShapeWithStrokeLinecap {
    private readonly static Anchor[] anchors = [
        new Anchor<Line>(s => s.StartPoint, (s, p) => s.StartPoint = p),
        new Anchor<Line>(s => s.EndPoint, (s, p) => s.EndPoint = p)
    ];

    public override string ElementName => "line";
    public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
    public Point StartPoint { get; set; }
    public Point EndPoint { get; set; }

    private Line() : this(new Point(0, 0), new Point(0, 0)) { }

    public Line(Point startPoint, Point endPoint) {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly([StartPoint, EndPoint]);

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x1", StartPoint.X },
        { "y1", StartPoint.Y },
        { "x2", EndPoint.X },
        { "y2", EndPoint.Y }
    };

    protected override Shape CreateClone() => new Line(StartPoint, EndPoint);

    public override Geometry GetGeometry(GeometryFactory geometryFactory, Point origin)
        => geometryFactory.CreateLineString([
            geometryFactory.GetCoordinate(StartPoint.X, StartPoint.Y, origin),
            geometryFactory.GetCoordinate(EndPoint.X, EndPoint.Y, origin)
        ]);

    public override BoundingBox GetBoundingBox()
        => new(
            new(Math.Min(StartPoint.X, EndPoint.X), Math.Min(StartPoint.Y, EndPoint.Y)),
            new(Math.Max(StartPoint.X, EndPoint.X), Math.Max(StartPoint.Y, EndPoint.Y))
        );
}
