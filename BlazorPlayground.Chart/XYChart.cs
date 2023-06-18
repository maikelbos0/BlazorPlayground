namespace BlazorPlayground.Chart;

public class XYChart {
    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();
    public YAxis YAxis { get; set; } = new();

    public void AutoScale() => YAxis.AutoScale(DataSeries.SelectMany(dataSeries => dataSeries.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value)));
}
