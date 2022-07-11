namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public abstract ShapeRenderType RenderType { get; }

        public abstract IEnumerable<Point> GetPoints();

        public abstract IEnumerable<Anchor> GetAnchors();

        // TODO test below
        public abstract Shape Clone();

        public void Transform(Point delta) => Transform(delta, GetAnchors().ToArray());

        public void Transform(Point delta, params Anchor[] anchors) {
            foreach (var anchor in anchors) {
                anchor.Set(anchor.Get() + delta);
            }
        }
    }
}
