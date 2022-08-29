using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    // TODO what of this can be either internal or needs validation
    public abstract class Shape {
        public IPaintServer Fill { get; set; } = PaintServer.None;
        public IPaintServer Stroke { get; set; } = PaintManager.ParseColor(DrawSettings.DefaultStrokeColor);
        public int StrokeWidth { get; set; } = DrawSettings.DefaultStrokeWidth;
        public Linecap StrokeLinecap { get; set; } = DrawSettings.DefaultStrokeLinecap;
        public Linejoin StrokeLinejoin { get; set; } = DrawSettings.DefaultStrokeLinejoin;
        public int Sides { get; set; } = DrawSettings.DefaultSides;
        public ShapeDefinition Definition => ShapeDefinition.Get(this);
        public abstract string ElementName { get; }
        public abstract IReadOnlyList<Anchor> Anchors { get; }

        public abstract ShapeAttributeCollection GetAttributes();

        protected abstract Shape CreateClone();

        public void Transform(Point delta, bool snapToGrid, int gridSize) {
            if (snapToGrid) {
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

        public virtual XElement CreateSvgElement() => new(ElementName, CreateSvgAttributes());

        private IEnumerable<XAttribute> CreateSvgAttributes() {
            if (Definition.UseFill) {
                yield return new XAttribute("fill", Fill);
            }

            yield return new XAttribute("stroke", Stroke);
            yield return new XAttribute("stroke-width", StrokeWidth);

            if (Definition.UseStrokeLinecap) {
                yield return new XAttribute("stroke-linecap", StrokeLinecap.ToString().ToLower());
            }

            if (Definition.UseStrokeLinejoin) {
                yield return new XAttribute("stroke-linejoin", StrokeLinejoin.ToString().ToLower());
            }

            foreach (var attribute in GetAttributes()) {
                yield return new XAttribute(attribute.Key, attribute.Value);
            }

            yield return new XAttribute("data-shape-type", GetType().Name);

            for (var i = 0; i < Anchors.Count; i++) {
                var point = Anchors[i].Get(this);

                yield return new XAttribute($"data-shape-anchor-{i}", FormattableString.Invariant($"{point.X},{point.Y}"));
            }

            if (Definition.UseSides) {
                yield return new XAttribute("data-shape-sides", Sides);
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
    }
}
