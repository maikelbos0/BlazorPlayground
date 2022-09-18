using Microsoft.AspNetCore.Components.Rendering;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public ShapeDefinition Definition => ShapeDefinition.Get(this);

        public abstract IReadOnlyList<Anchor> Anchors { get; }

        public abstract IReadOnlyList<Point> GetSnapPoints();

        public abstract void BuildRenderTree(RenderTreeBuilder builder);

        public abstract XElement CreateSvgElement();

        public void Transform(Point delta, bool snapToGrid, int gridSize, bool snapToPoints, IEnumerable<Point> points) {
            var point = GetSnapPoints()
                .Select(p => new { Original = p, Transformed = p + delta, Anchored = (p + delta).Snap(snapToGrid, gridSize, snapToPoints, points) })
                .OrderBy(p => (p.Transformed - p.Anchored).Distance)
                .FirstOrDefault();

            if (point != null) {
                delta = point.Anchored - point.Original;
            }

            foreach (var anchor in Anchors) {
                anchor.Move(this, delta);
            }
        }

        public Shape Clone() {
            var clone = CreateClone();

            if (this is IShapeWithOpacity shapeWithOpacity && clone is IShapeWithOpacity cloneWithOpacity) {
                cloneWithOpacity.SetOpacity(shapeWithOpacity.GetOpacity());
            }

            if (this is IShapeWithFill shapeWithFill && clone is IShapeWithFill cloneWithFill) {
                cloneWithFill.SetFill(shapeWithFill.GetFill());
                cloneWithFill.SetFillOpacity(shapeWithFill.GetFillOpacity());
            }

            if (this is IShapeWithStroke shapeWithStroke && clone is IShapeWithStroke cloneWithStroke) {
                cloneWithStroke.SetStroke(shapeWithStroke.GetStroke());
                cloneWithStroke.SetStrokeWidth(shapeWithStroke.GetStrokeWidth());
                cloneWithStroke.SetStrokeOpacity(shapeWithStroke.GetStrokeOpacity());
            }

            if (this is IShapeWithStrokeLinecap shapeWithStrokeLinecap && clone is IShapeWithStrokeLinecap cloneWithStrokeLinecap) {
                cloneWithStrokeLinecap.SetStrokeLinecap(shapeWithStrokeLinecap.GetStrokeLinecap());
            }

            if (this is IShapeWithStrokeLinejoin shapeWithStrokeLinejoin && clone is IShapeWithStrokeLinejoin cloneWithStrokeLinejoin) {
                cloneWithStrokeLinejoin.SetStrokeLinejoin(shapeWithStrokeLinejoin.GetStrokeLinejoin());
            }

            if (this is IShapeWithSides shapeWithSides && clone is IShapeWithSides cloneWithSides) {
                cloneWithSides.SetSides(shapeWithSides.GetSides());
            }

            return clone;
        }

        protected abstract Shape CreateClone();
    }
}
