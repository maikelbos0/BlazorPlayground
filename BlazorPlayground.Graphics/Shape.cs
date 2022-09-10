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

        public IPaintServer Fill { get; set; } = PaintServer.None;

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

        // TODO add snap to shapes
        public void Transform(Point delta, bool snapToGrid, int gridSize) {
            if (snapToGrid) {
                // TODO work with snap points rather than anchors
                var point = Anchors
                    .Select(a => a.Get(this))
                    .Select(p => new { Original = p, Transformed = p + delta, Anchored = (p + delta).SnapToGrid(gridSize) })
                    .OrderBy(p => (p.Transformed - p.Anchored).Distance)
                    .First();

                delta = point.Anchored - point.Original;
            }

            foreach (var anchor in Anchors) {
                anchor.Move(this, delta);
            }
        }

        public Shape Clone() {
            var clone = CreateClone();

            clone.Fill = Fill;
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
