namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public abstract ShapeRenderType RenderType { get; }

        public abstract IEnumerable<Point> GetPoints();

        public abstract IEnumerable<Anchor> GetAnchors();

        // TODO test below
        public abstract Shape Clone();

        public void Transform(Point delta) {
            foreach (var anchor in GetAnchors()) {
                anchor.Move(delta);
            }
        }
    }
}
