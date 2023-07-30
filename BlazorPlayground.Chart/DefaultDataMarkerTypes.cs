using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public static class DefaultDataMarkerTypes {
    public static ShapeBase Round(decimal x, decimal y, decimal size, string color, int dataSeriesIndex, int dataPointIndex)
        => new RoundDataMarkerShape(x, y, size, color, dataSeriesIndex, dataPointIndex);

    public static ShapeBase Square(decimal x, decimal y, decimal size, string color, int dataSeriesIndex, int dataPointIndex)
        => new SquareDataMarkerShape(x, y, size, color, dataSeriesIndex, dataPointIndex);
}
