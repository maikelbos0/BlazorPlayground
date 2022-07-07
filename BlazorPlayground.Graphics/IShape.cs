namespace BlazorPlayground.Graphics {
    public interface IShape {
        ShapeRenderType GetSeriesType();
        IEnumerable<Point> GetPoints();
    }
}
