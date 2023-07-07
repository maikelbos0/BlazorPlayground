namespace BlazorPlayground.Chart;

public class AutoScaleSettings {
    public static bool DefaultIsEnabled { get; set; } = true;
    public static bool DefaultIncludeZero { get; set; } = false;

    public bool IsEnabled { get; set; } = DefaultIsEnabled;
    public bool IncludeZero { get; set; } = DefaultIncludeZero;
}
