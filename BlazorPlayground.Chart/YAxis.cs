namespace BlazorPlayground.Chart;

public class YAxis {
    public YAxis AutoScale() {
        throw new NotImplementedException();
    }

    public double Min { get; }
    public double Max { get; }
    public double GridLineInterval { get; }

    public YAxis(double gridLineInterval, double min, double max) {
        GridLineInterval = gridLineInterval;
        Min = min;
        Max = max;
    }

    public IEnumerable<double> GetGridLines() {
        var gridLine = Min.CeilingToScale(GridLineInterval);

        while (gridLine < Max) {
            yield return gridLine;

            gridLine += GridLineInterval;
        }
    }
}
