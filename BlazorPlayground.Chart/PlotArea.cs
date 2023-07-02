namespace BlazorPlayground.Chart;

public class PlotArea {
    public static decimal DefaultMin { get; set; } = 0M;
    public static decimal DefaultMax { get; set; } = 5M;
    public static decimal DefaultGridLineInterval { get; set; } = 0.5M;

    public decimal Min { get; set; } = DefaultMin;
    public decimal Max { get; set; } = DefaultMax;
    public decimal GridLineInterval { get; set; } = DefaultGridLineInterval;

    public void AutoScale(IEnumerable<decimal> dataPoints, int requestedGridLineCount) {
        var min = dataPoints.DefaultIfEmpty(DefaultMin).Min();
        var max = dataPoints.DefaultIfEmpty(DefaultMax).Max();

        if (min == max) {
            min -= (DefaultMax - DefaultMin) / 2M;
            max += (DefaultMax - DefaultMin) / 2M;
        }

        // TODO force 0 line?
        // TODO adjust so data points are always *in between* min and max, not exactly on either

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

        Min = DecimalMath.Trim(scale.Min);
        Max = DecimalMath.Trim(scale.Max);
        GridLineInterval = DecimalMath.Trim(scale.GridLineInterval);
    }

    public IEnumerable<decimal> GetGridLineDataPoints() {
        var dataPoint = DecimalMath.CeilingToScale(Min, GridLineInterval);

        while (dataPoint <= Max) {
            yield return dataPoint;

            dataPoint += GridLineInterval;
        }
    }
}
