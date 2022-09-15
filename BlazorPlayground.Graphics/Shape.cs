using Microsoft.AspNetCore.Components.Rendering;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        private int opacity = DrawSettings.DefaultOpacity;
        private int strokeWidth = DrawSettings.DefaultStrokeWidth;
        private int sides = DrawSettings.DefaultSides;

        public int Opacity {
            get => opacity;
            set => opacity = Math.Max(Math.Min(value, DrawSettings.MaximumOpacity), DrawSettings.MinimumOpacity);
        }

        public IPaintServer Stroke { get; set; } = PaintManager.ParseColor(DrawSettings.DefaultStrokeColor);

        public int StrokeWidth {
            get => strokeWidth;
            set => strokeWidth = Math.Max(value, DrawSettings.MinimumStrokeWidth);
        }

        public Linecap StrokeLinecap { get; set; } = DrawSettings.DefaultStrokeLinecap;

        public Linejoin StrokeLinejoin { get; set; } = DrawSettings.DefaultStrokeLinejoin;

        public int Sides {
            get => sides;
            set => sides = Math.Max(value, DrawSettings.MinimumSides);
        }

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

            if (this is IShapeWithFill shapeWithFill && clone is IShapeWithFill cloneWithFill) {
                cloneWithFill.SetFill(shapeWithFill.GetFill());
            }

            clone.Stroke = Stroke;
            clone.StrokeWidth = StrokeWidth;
            clone.StrokeLinecap = StrokeLinecap;
            clone.StrokeLinejoin = StrokeLinejoin;
            clone.Sides = Sides;

            return clone;
        }

        protected abstract Shape CreateClone();
    }
}
