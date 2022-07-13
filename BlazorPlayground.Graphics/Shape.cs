namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public abstract ShapeRenderType RenderType { get; }
        public abstract IReadOnlyList<Anchor> Anchors { get; }

        public abstract IEnumerable<Point> GetPoints();

        public abstract Shape Clone();

        // TODO test below
        public void Transform(Point delta) {
            foreach (var anchor in Anchors) {
                anchor.Move(this, delta);
            }
        }
    }
}
