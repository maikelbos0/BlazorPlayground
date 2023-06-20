namespace BlazorPlayground.Chart;

public class XYChart {
    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();
    public XAxis XAxis { get; set; } = new();
    public YAxis YAxis { get; set; } = new();

    public int Width { get; set; } = 1200;
    public int Height { get; set; } = 600;
    public int Padding { get; set; } = 10;

    public void AutoScale() => YAxis.AutoScale(DataSeries.SelectMany(dataSeries => dataSeries.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value)));
    internal PlotArea GetPlotArea() => new(Padding + YAxis.Size, Padding, Width - Padding * 2 - YAxis.Size, Height - Padding * 2 - XAxis.Size);
}
