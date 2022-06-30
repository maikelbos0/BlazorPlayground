namespace BlazorPlayground.Graphics {
    public interface IShape {
        RenderType GetSeriesType();
        IEnumerable<Point> GetPoints();
    }
}
