namespace BlazorPlayground.Chart;

public class PlotArea {
    public static double DefaultMin { get; set; } = 0;
    public static double DefaultMax { get; set; } = 5;
    public static double DefaultGridLineInterval { get; set; } = 1;

    public double Min { get; set; } = DefaultMin;
    public double Max { get; set; } = DefaultMax;
    public double GridLineInterval { get; set; } = DefaultGridLineInterval;

    public void AutoScale(IEnumerable<double> dataPoints) {
        const int maxGridLineCount = 8;
        double[] gridLineMultipliers = new[] { 2, 2.5, 2, 2 };
        var i = 0;

        Min = dataPoints.DefaultIfEmpty(DefaultMin).Min();
        Max = dataPoints.DefaultIfEmpty(DefaultMax).Max();

        if (Min == Max) {
            Min -= (DefaultMax - DefaultMin) / 2;
            Max += (DefaultMax - DefaultMin) / 2;
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

        while (gridLine <= Max) {
            yield return gridLine;

            gridLine += GridLineInterval;
        }
    }
}
