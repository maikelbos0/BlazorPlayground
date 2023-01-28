namespace BlazorPlayground.Chart;

public class XYChart {
    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();
    public YAxis? YAxis { get; set; }
}
