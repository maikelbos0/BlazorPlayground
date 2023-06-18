namespace BlazorPlayground.Chart;

public class XYChart {
    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();
    public XAxis XAxis { get; set; } = new();
    public YAxis YAxis { get; set; } = new();

    public int CanvasWidth { get; set; } = 1200;
    public int CanvasHeight { get; set; } = 600;

    public void AutoScale() => YAxis.AutoScale(DataSeries.SelectMany(dataSeries => dataSeries.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value)));
}
