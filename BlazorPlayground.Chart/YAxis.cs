namespace BlazorPlayground.Chart;

public class YAxis : AxisBase {
    private const double defaultMin = 0;
    private const double defaultMax = 5;
    private const double defaultMiddle = (defaultMax - defaultMin) / 2;
    private const double defaultGridLineInterval = 1;
    private const int maxGridLineCount = 8;
    private readonly static double[] gridLineMultipliers = new[] { 2, 2.5, 2, 2 };

    public double Min { get; set; } = defaultMin;
    public double Max { get; set; } = defaultMax;
    public double GridLineInterval { get; set; } = defaultGridLineInterval;

    public void AutoScale(IEnumerable<double> dataPoints) {
        var i = 0;

        Min = dataPoints.DefaultIfEmpty(defaultMin).Min();
        Max = dataPoints.DefaultIfEmpty(defaultMax).Max();

        if (Min == Max) {
            Min -= defaultMiddle;
            Max += defaultMiddle;
        }

        GridLineInterval = Math.Pow(10, Math.Floor(Math.Log10(Max - Min) - 1));

        while ((Max.CeilingToScale(GridLineInterval) - Min.FloorToScale(GridLineInterval)) / GridLineInterval > maxGridLineCount) {
            GridLineInterval *= gridLineMultipliers[i++];
        }

        Min = Min.FloorToScale(GridLineInterval);
        Max = Max.CeilingToScale(GridLineInterval);
    }

    public IEnumerable<double> GetGridLines() {
        var gridLine = Min.CeilingToScale(GridLineInterval);

        while (gridLine < Max) {
            yield return gridLine;

            gridLine += GridLineInterval;
        }
    }
}
