using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class PlotArea : ComponentBase, IDisposable {
    public static decimal DefaultMin { get; set; } = 0M;
    public static decimal DefaultMax { get; set; } = 50M;
    public static decimal DefaultGridLineInterval { get; set; } = 5M;
    public static decimal DefaultMultiplier { get; set; } = 1M;

    [CascadingParameter] internal XYChart Chart { get; set; } = null!;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public decimal Min { get; set; } = DefaultMin;
    [Parameter] public decimal Max { get; set; } = DefaultMax;
    [Parameter] public decimal GridLineInterval { get; set; } = DefaultGridLineInterval;
    [Parameter] public decimal Multiplier { get; set; } = DefaultMultiplier;
    public AutoScaleSettings AutoScaleSettings { get; set; } = new();

    protected override void OnInitialized() => Chart.SetPlotArea(this);

    public void Dispose() => Chart.ResetPlotArea();

    internal void SetAutoScaleSettings(AutoScaleSettings autoScaleSettings) {
        AutoScaleSettings = autoScaleSettings;
        Chart.HandleStateChange();
    }

    internal void ResetAutoScaleSettings() {
        AutoScaleSettings = new();
        Chart.HandleStateChange();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<PlotArea>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }

    public void AutoScale(IEnumerable<decimal> dataPoints) {
        if (!AutoScaleSettings.IsEnabled) {
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
                var clearance = (max - min) / (100M - AutoScaleSettings.ClearancePercentage * 2) * AutoScaleSettings.ClearancePercentage;

                min -= clearance;
                max += clearance;
            }

            if (min > 0M && AutoScaleSettings.IncludeZero) {
                min = 0M;
            }

            if (max < 0M && AutoScaleSettings.IncludeZero) {
                max = 0M;
            }
        }

        var rawGridLineInterval = (max - min) / Math.Max(1, AutoScaleSettings.RequestedGridLineCount - 1);
        var baseMultiplier = DecimalMath.Pow(10M, (int)Math.Floor((decimal)Math.Log10((double)rawGridLineInterval)));
        var scale = new[] { 1M, 2M, 5M, 10M }
            .Select(baseGridLineInterval => baseGridLineInterval * baseMultiplier)
            .Select(gridLineInterval => new {
                GridLineInterval = gridLineInterval,
                Min = DecimalMath.FloorToScale(min, gridLineInterval),
                Max = DecimalMath.CeilingToScale(max, gridLineInterval)
            })
            .OrderBy(candidate => Math.Abs((candidate.Max - candidate.Min) / candidate.GridLineInterval - AutoScaleSettings.RequestedGridLineCount))
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
