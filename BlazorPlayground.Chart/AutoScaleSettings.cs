namespace BlazorPlayground.Chart;

public class AutoScaleSettings {
    public static bool DefaultIsEnabled { get; set; } = true;
    public static int DefaultRequestedGridLineCount { get; set; } = 11;
    public static bool DefaultIncludeZero { get; set; } = false;
    public static decimal DefaultClearancePercentage { get; set; } = 5M;

    public bool IsEnabled { get; set; } = DefaultIsEnabled;
    public int RequestedGridLineCount { get; set; } = DefaultRequestedGridLineCount;
    public bool IncludeZero { get; set; } = DefaultIncludeZero;
    public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;
}
