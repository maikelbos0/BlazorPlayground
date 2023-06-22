namespace BlazorPlayground.Chart;

public class XYChart {
    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();
    public XAxis XAxis { get; set; } = new();
    public YAxis YAxis { get; set; } = new();

    public int Width { get; set; } = 1200;
    public int Height { get; set; } = 600;
    public int Padding { get; set; } = 10;
    public int PlotAreaWidth => Width - Padding * 2 - YAxis.Size;
    public int PlotAreaHeight => Height - Padding * 2 - XAxis.Size;

    public void AutoScale() => YAxis.AutoScale(DataSeries.SelectMany(dataSeries => dataSeries.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value)));

    public IEnumerable<Shape> GetShapes() {
        yield return GetPlotArea();

        foreach (var gridLine in GetGridLines()) {
            yield return gridLine;
        }
    }

    public PlotArea GetPlotArea() => new(Padding + YAxis.Size, Padding, PlotAreaWidth, PlotAreaHeight);

    public IEnumerable<GridLine> GetGridLines() => YAxis.GetGridLines().Select(y => new GridLine(Padding + YAxis.Size, Padding + MapToPlotArea(y), PlotAreaWidth));

    public double MapToPlotArea(double dataPoint) => (dataPoint - YAxis.Min) / (YAxis.Max - YAxis.Min) * PlotAreaHeight;
}
