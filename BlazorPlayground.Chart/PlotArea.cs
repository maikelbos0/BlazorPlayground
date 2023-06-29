namespace BlazorPlayground.Chart;

public class PlotArea {
    public static decimal DefaultMin { get; set; } = 0M;
    public static decimal DefaultMax { get; set; } = 5M;
    public static decimal DefaultGridLineInterval { get; set; } = 1M;

    public decimal Min { get; set; } = DefaultMin;
    public decimal Max { get; set; } = DefaultMax;
    public decimal GridLineInterval { get; set; } = DefaultGridLineInterval;

    public void AutoScale(IEnumerable<decimal> dataPoints) {
        const int maxGridLineCount = 8;
        decimal[] gridLineMultipliers = new[] { 2M, 2.5M, 2M, 2M };
        var i = 0;

        Min = dataPoints.DefaultIfEmpty(DefaultMin).Min();
        Max = dataPoints.DefaultIfEmpty(DefaultMax).Max();

        if (Min == Max) {
            Min -= (DefaultMax - DefaultMin) / 2M;
            Max += (DefaultMax - DefaultMin) / 2M;
        }

        var gridLineIntervalDecimalShift = (int)Math.Log10((double)(Max - Min)) - 1;

        GridLineInterval = 1;

        if (gridLineIntervalDecimalShift > 0) {
            for (var _ = 0; _ < gridLineIntervalDecimalShift; _++) {
                GridLineInterval *= 10M;
            }
        }
        else if (gridLineIntervalDecimalShift < 0) {
            for (var _ = 0; _ > gridLineIntervalDecimalShift; _--) {
                GridLineInterval *= 0.1M;
            }
        }

        while ((DecimalMath.CeilingToScale(Max, GridLineInterval) - DecimalMath.FloorToScale(Min, GridLineInterval)) / GridLineInterval > maxGridLineCount) {
            GridLineInterval *= gridLineMultipliers[i++];
        }

        Min = DecimalMath.FloorToScale(Min, GridLineInterval);
        Max = DecimalMath.CeilingToScale(Max, GridLineInterval);
    }

    public IEnumerable<decimal> GetGridLineDataPoints() {
        var dataPoint = DecimalMath.CeilingToScale(Min, GridLineInterval);

        while (dataPoint <= Max) {
            yield return dataPoint;

            dataPoint += GridLineInterval;
        }
    }
}
