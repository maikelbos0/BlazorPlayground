using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public IPaintServer Fill { get; set; } = PaintServer.None;
        public IPaintServer Stroke { get; set; } = new Color(0, 0, 0, 1);
        public int StrokeWidth { get; set; } = 1;
        public Linecap StrokeLinecap { get; set; } = Linecap.Butt;
        public Linejoin StrokeLinejoin { get; set; } = Linejoin.Miter;
        public int Sides { get; set; } = 3;
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

        public virtual XElement CreateElement() {
            var element = new XElement(
                ElementName,
                new XAttribute("fill", Fill),
                new XAttribute("stroke", Stroke),
                new XAttribute("stroke-width", StrokeWidth),
                new XAttribute("stroke-linecap", StrokeLinecap.ToString().ToLower()),
                new XAttribute("stroke-linejoin", StrokeLinejoin.ToString().ToLower())
            );

            element.Add(GetAttributes().Select(a => new XAttribute(a.Key, a.Value)));

            return element;
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
