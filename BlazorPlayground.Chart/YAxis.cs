namespace BlazorPlayground.Chart;

public class YAxis {
    public static YAxis AutoScale(IEnumerable<double> dataPoints) {
        const double defaultMin = 0;
        const double defaultMax = 5;
        const double defaultMiddle = (defaultMax - defaultMin) / 2;
        const int maxGridLineCount = 8;
        double[] gridLineMultipliers = new[] { 2, 2.5, 2, 2 };

        var min = dataPoints.DefaultIfEmpty(defaultMin).Min();
        var max = dataPoints.DefaultIfEmpty(defaultMax).Max();

        if (min == max) {
            min -= defaultMiddle;
            max += defaultMiddle;
        }

        var gridLineInterval = Math.Pow(10, Math.Floor(Math.Log10(max - min) - 1));
        var i = 0;

        while ((max.CeilingToScale(gridLineInterval) - min.FloorToScale(gridLineInterval)) / gridLineInterval > maxGridLineCount) {
            gridLineInterval *= gridLineMultipliers[i++];
        }

        return new YAxis(gridLineInterval, min, max);
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
