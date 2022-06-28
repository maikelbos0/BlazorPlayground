namespace BlazorPlayground.Graphics {
    public interface IPointSeries {
        PointSeriesType GetSeriesType();
        IEnumerable<Point> GetPoints();
    }
}
