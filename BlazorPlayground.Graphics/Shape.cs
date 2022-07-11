namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public bool IsSelected { get; set; } = false;
        public abstract ShapeRenderType RenderType { get; }
        public abstract IEnumerable<Point> GetPoints();
        public abstract IEnumerable<Anchor> GetAnchors();
    }
}
