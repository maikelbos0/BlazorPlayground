namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public ShapeRenderMode RenderMode { get; set; } = ShapeRenderMode.Default;
        public abstract ShapeRenderType RenderType { get; }
        public abstract IEnumerable<Point> GetPoints();
    }
}
