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

    public void AutoScale(IEnumerable<decimal> dataPoints, int requestedGridLineCount) {
        var min = dataPoints.DefaultIfEmpty(DefaultMin).Min();
        var max = dataPoints.DefaultIfEmpty(DefaultMax).Max();

        if (min == max) {
            min -= (DefaultMax - DefaultMin) / 2M;
            max += (DefaultMax - DefaultMin) / 2M;
        }

        // TODO force 0 line?

        var rawGridLineInterval = (max - min) / Math.Max(1, requestedGridLineCount - 1);
        var baseMultiplier = DecimalMath.Pow(10M, (int)Math.Floor((decimal)Math.Log10((double)rawGridLineInterval)));
        var scale = new[] { 1M, 2M, 5M, 10M }
            .Select(baseGridLineInterval => baseGridLineInterval * baseMultiplier)
            .Select(gridLineInterval => new {
                GridLineInterval = gridLineInterval,
                Min = DecimalMath.FloorToScale(min, gridLineInterval),
                Max = DecimalMath.CeilingToScale(max, gridLineInterval)
            })
            .OrderBy(candidate => Math.Abs((candidate.Max - candidate.Min) / candidate.GridLineInterval - requestedGridLineCount))
            .First();

        Min = scale.Min;
        Max = scale.Max;
        GridLineInterval = scale.GridLineInterval;
    }

    public IEnumerable<decimal> GetGridLineDataPoints() {
        var dataPoint = DecimalMath.CeilingToScale(Min, GridLineInterval);

        while (dataPoint <= Max) {
            yield return dataPoint;

            dataPoint += GridLineInterval;
        }
    }
}
