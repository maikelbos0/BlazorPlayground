using Microsoft.AspNetCore.Components;

namespace BlazorPlayground.Chart;

public class AutoScaleSettings : ComponentBase, IDisposable {
    public static bool DefaultIsEnabled { get; set; } = true;
    public static int DefaultRequestedGridLineCount { get; set; } = 11;
    public static bool DefaultIncludeZero { get; set; } = false;
    public static decimal DefaultClearancePercentage { get; set; } = 5M;

    [CascadingParameter] internal PlotArea PlotArea { get; set; } = null!;

    [Parameter] public bool IsEnabled { get; set; } = DefaultIsEnabled;
    [Parameter] public int RequestedGridLineCount { get; set; } = DefaultRequestedGridLineCount;
    [Parameter] public bool IncludeZero { get; set; } = DefaultIncludeZero;
    [Parameter] public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;

    protected override void OnInitialized() => PlotArea.SetAutoScaleSettings(this);

    public void Dispose() => PlotArea.ResetAutoScaleSettings();
}
