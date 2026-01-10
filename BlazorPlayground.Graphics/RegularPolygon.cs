using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorPlayground.Graphics;

public class RegularPolygon : DrawableShape, IShapeWithOpacity, IShapeWithFill, IShapeWithStroke, IShapeWithSides, IShapeWithStrokeLinejoin {
    private readonly static Anchor[] anchors = [
        new Anchor<RegularPolygon>(s => s.CenterPoint, (s, p) => s.CenterPoint = p),
        new Anchor<RegularPolygon>(s => s.RadiusPoint, (s, p) => s.RadiusPoint = p)
    ];

    public override string ElementName => "polygon";
    public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
    public Point CenterPoint { get; set; }
    public Point RadiusPoint { get; set; }

    private RegularPolygon() : this(new Point(0, 0), new Point(0, 0)) { }

    public RegularPolygon(Point centerPoint, Point radiusPoint) {
        CenterPoint = centerPoint;
        RadiusPoint = radiusPoint;
    }

    public override IReadOnlyList<Point> GetSnapPoints() => Array.AsReadOnly(GetPoints().Append(CenterPoint).ToArray());

    public IEnumerable<Point> GetPoints() {
        var vector = RadiusPoint - CenterPoint;
        var radius = Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        var startingAngle = Math.Atan2(vector.Y, vector.X);
        var pointAngle = Math.PI / this.Sides * 2;

        for (var i = 0; i < this.Sides; i++) {
            var angle = startingAngle + pointAngle * i;

            yield return CenterPoint + new Point(radius * Math.Cos(angle), radius * Math.Sin(angle));
        }
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "points", string.Join(" ", GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))) }
    };

    protected override Shape CreateClone() => new RegularPolygon(CenterPoint, RadiusPoint);

    public override Geometry GetGeometry(GeometryFactory geometryFactory, Point origin) {
        var coordinates = new Coordinate[this.Sides + 1];

        foreach (var (point, i) in GetPoints().Select((point, i) => (point, i))) {
            coordinates[i] = geometryFactory.GetCoordinate(point.X, point.Y, origin);
        }

        coordinates[^1] = coordinates[0];

        return geometryFactory.CreatePolygon([.. coordinates]);
    }

    public override BoundingBox GetBoundingBox() {
        var radius = (CenterPoint - RadiusPoint).Distance;

        return new(
            new(CenterPoint.X - radius, CenterPoint.Y - radius),
            new(CenterPoint.X + radius, CenterPoint.Y + radius)
        );
    }
}
