using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;

namespace BlazorPlayground.Graphics;

public class Ellipse : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke {
    private readonly static Anchor[] anchors = [
        new Anchor<Ellipse>(s => s.CenterPoint, (s, p) => s.CenterPoint = p),
        new Anchor<Ellipse>(s => s.RadiusPoint, (s, p) => s.RadiusPoint = p)
    ];

    public override string ElementName => "ellipse";
    public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
    public Point CenterPoint { get; set; }
    public Point RadiusPoint { get; set; }

    private Ellipse() : this(new Point(0, 0), new Point(0, 0)) { }

    public Ellipse(Point centerPoint, Point radiusPoint) {
        CenterPoint = centerPoint;
        RadiusPoint = radiusPoint;
    }

    public override IReadOnlyList<Point> GetSnapPoints() {
        var delta = CenterPoint - RadiusPoint;

        return Array.AsReadOnly([
            CenterPoint,
            new Point(CenterPoint.X, CenterPoint.Y + delta.Y),
            new Point(CenterPoint.X, CenterPoint.Y - delta.Y),
            new Point(CenterPoint.X + delta.X, CenterPoint.Y),
            new Point(CenterPoint.X - delta.X, CenterPoint.Y),
        ]);
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "cx", CenterPoint.X },
        { "cy", CenterPoint.Y },
        { "rx", Math.Abs(RadiusPoint.X - CenterPoint.X) },
        { "ry", Math.Abs(RadiusPoint.Y - CenterPoint.Y) }
    };

    protected override Shape CreateClone() => new Ellipse(CenterPoint, RadiusPoint);

    public override Geometry GetGeometry(GeometryFactory geometryFactory, Point origin) {
        var radiusX = Math.Abs(CenterPoint.X - RadiusPoint.X);
        var radiusY = Math.Abs(CenterPoint.Y - RadiusPoint.Y);
        var coordinates = new Coordinate[CurveApproximationSegmentCount + 1];

        for (var i = 0; i < CurveApproximationSegmentCount; i++) {
            var angle = CurveAproximationAngleIncrement * i;
            var dx = radiusX * Math.Cos(angle);
            var dy = radiusY * Math.Sin(angle);
            coordinates[i] = geometryFactory.GetCoordinate(CenterPoint.X + dx, CenterPoint.Y + dy, origin);
        }

        coordinates[^1] = coordinates[0];

        return geometryFactory.CreatePolygon(coordinates);
    }

    public override BoundingBox GetBoundingBox() {
        var radiusX = Math.Abs(CenterPoint.X - RadiusPoint.X);
        var radiusY = Math.Abs(CenterPoint.Y - RadiusPoint.Y);

        return new(
            new(CenterPoint.X - radiusX, CenterPoint.Y - radiusY),
            new(CenterPoint.X + radiusX, CenterPoint.Y + radiusY)
        );
    }
}
