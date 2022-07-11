namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public abstract ShapeRenderType RenderType { get; }
        public abstract IEnumerable<Point> GetPoints();
        public abstract IEnumerable<Anchor> GetAnchors();
    }
}
