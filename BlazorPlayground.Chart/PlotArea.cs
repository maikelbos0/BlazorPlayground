namespace BlazorPlayground.Chart;

public class PlotArea {
    public static decimal DefaultMin { get; set; } = 0M;
    public static decimal DefaultMax { get; set; } = 5M;
    public static decimal DefaultGridLineInterval { get; set; } = 0.5M;

    public decimal Min { get; set; } = DefaultMin;
    public decimal Max { get; set; } = DefaultMax;
    public decimal GridLineInterval { get; set; } = DefaultGridLineInterval;

    public void AutoScale(AutoScaleSettings settings, IEnumerable<decimal> dataPoints) {
        if (!settings.IsEnabled) {
            return;
        }

        var min = DefaultMin;
        var max = DefaultMax;

        if (dataPoints.Any()) {
            min = dataPoints.Min();
            max = dataPoints.Max();

            if (min == max) {
                min -= (DefaultMax - DefaultMin) / 2M;
                max += (DefaultMax - DefaultMin) / 2M;
            }
            else {
                var clearance = (max - min) / (100M - settings.ClearancePercentage * 2) * settings.ClearancePercentage;

                min -= clearance;
                max += clearance;
            }

            if (min > 0M && settings.IncludeZero) {
                min = 0M;
            }

            if (max < 0M && settings.IncludeZero) {
                max = 0M;
            }
        }

        var rawGridLineInterval = (max - min) / Math.Max(1, settings.RequestedGridLineCount - 1);
        var baseMultiplier = DecimalMath.Pow(10M, (int)Math.Floor((decimal)Math.Log10((double)rawGridLineInterval)));
        var scale = new[] { 1M, 2M, 5M, 10M }
            .Select(baseGridLineInterval => baseGridLineInterval * baseMultiplier)
            .Select(gridLineInterval => new {
                GridLineInterval = gridLineInterval,
                Min = DecimalMath.FloorToScale(min, gridLineInterval),
                Max = DecimalMath.CeilingToScale(max, gridLineInterval)
            })
            .OrderBy(candidate => Math.Abs((candidate.Max - candidate.Min) / candidate.GridLineInterval - settings.RequestedGridLineCount))
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
