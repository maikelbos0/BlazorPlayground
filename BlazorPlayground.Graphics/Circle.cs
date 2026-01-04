using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace BlazorPlayground.Graphics;

public class Circle : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke {
    private readonly static Anchor[] anchors = [
        new Anchor<Circle>(s => s.CenterPoint, (s, p) => s.CenterPoint = p),
        new Anchor<Circle>(s => s.RadiusPoint, (s, p) => s.RadiusPoint = p)
    ];

    public override string ElementName => "circle";
    public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
    public Point CenterPoint { get; set; }
    public Point RadiusPoint { get; set; }

    private Circle() : this(new Point(0, 0), new Point(0, 0)) { }

    public Circle(Point centerPoint, Point radiusPoint) {
        CenterPoint = centerPoint;
        RadiusPoint = radiusPoint;
    }

    public override IReadOnlyList<Point> GetSnapPoints() {
        var delta = CenterPoint - RadiusPoint;

        return Array.AsReadOnly([
            CenterPoint,
            RadiusPoint,
            new Point(CenterPoint.X + delta.X, CenterPoint.Y + delta.Y),
            new Point(CenterPoint.X + delta.Y, CenterPoint.Y - delta.X),
            new Point(CenterPoint.X - delta.Y, CenterPoint.Y + delta.X),
        ]);
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "cx", CenterPoint.X },
        { "cy", CenterPoint.Y },
        { "r", (RadiusPoint - CenterPoint).Distance }
    };

    protected override Shape CreateClone() => new Circle(CenterPoint, RadiusPoint);

    public override Geometry GetGeometry(GeometryFactory geometryFactory, Point origin) {
        var radius = (CenterPoint - RadiusPoint).Distance;
        var coordinates = new Coordinate[CurveApproximationSegmentCount + 1];

        for (var i = 0; i < CurveApproximationSegmentCount; i++) {
            var angle = CurveAproximationAngleIncrement * i;
            var dx = radius * Math.Cos(angle);
            var dy = radius * Math.Sin(angle);
            coordinates[i] = geometryFactory.GetCoordinate(CenterPoint.X + dx, CenterPoint.Y + dy, origin);
        }

        coordinates[^1] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    public override BoundingBox GetBoundingBox() {
        var radius = (CenterPoint - RadiusPoint).Distance;

        return new(
            CenterPoint.X - radius,
            CenterPoint.X + radius,
            CenterPoint.Y - radius,
            CenterPoint.Y + radius
        );
    }
}
