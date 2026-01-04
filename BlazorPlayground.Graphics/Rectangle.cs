using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace BlazorPlayground.Graphics;

public class Rectangle : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithStrokeLinejoin {
    private readonly static Anchor[] anchors = [
        new Anchor<Rectangle>(s => s.StartPoint, (s, p) => s.StartPoint = p),
        new Anchor<Rectangle>(s => s.EndPoint, (s, p) => s.EndPoint = p)
    ];

    public override string ElementName => "rect";
    public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
    public Point StartPoint { get; set; }
    public Point EndPoint { get; set; }

    private Rectangle() : this(new Point(0, 0), new Point(0, 0)) { }

    public Rectangle(Point startPoint, Point endPoint) {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }

    public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly([StartPoint, EndPoint, new Point(StartPoint.X, EndPoint.Y), new Point(EndPoint.X, StartPoint.Y)]);

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", Math.Min(StartPoint.X, EndPoint.X) },
        { "y", Math.Min(StartPoint.Y, EndPoint.Y) },
        { "width", Math.Abs(StartPoint.X - EndPoint.X) },
        { "height", Math.Abs(StartPoint.Y - EndPoint.Y) }
    };

    protected override Shape CreateClone() => new Rectangle(StartPoint, EndPoint);

    public override Geometry GetGeometry(GeometryFactory geometryFactory, Point origin)
        => geometryFactory.CreatePolygon([
            geometryFactory.GetCoordinate(StartPoint.X, StartPoint.Y, origin),
            geometryFactory.GetCoordinate(StartPoint.X, EndPoint.Y, origin),
            geometryFactory.GetCoordinate(EndPoint.X, EndPoint.Y, origin),
            geometryFactory.GetCoordinate(EndPoint.X, StartPoint.Y, origin),
            geometryFactory.GetCoordinate(StartPoint.X, StartPoint.Y, origin)
        ]);

    public override BoundingBox GetBoundingBox()
        => new(
            Math.Min(StartPoint.X, EndPoint.X),
            Math.Max(StartPoint.X, EndPoint.X),
            Math.Min(StartPoint.Y, EndPoint.Y),
            Math.Max(StartPoint.Y, EndPoint.Y)
        );
}