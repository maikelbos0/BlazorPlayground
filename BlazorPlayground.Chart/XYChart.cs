namespace BlazorPlayground.Chart;

public class XYChart {
    public Canvas Canvas = new();
    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();
    public YAxis YAxis { get; set; } = new();

    public void AutoScale() => YAxis.AutoScale(DataSeries.SelectMany(dataSeries => dataSeries.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value)));

    public IEnumerable<Shapes.Shape> GetShapes() {
        yield return GetPlotArea();

        foreach (var gridLine in GetGridLines()) {
            yield return gridLine;
        }
    }

    public Shapes.PlotArea GetPlotArea() => new(Canvas.PlotAreaX, Canvas.PlotAreaY, Canvas.PlotAreaWidth, Canvas.PlotAreaHeight);

    public IEnumerable<Shapes.GridLine> GetGridLines() => YAxis.GetGridLines().Select(y => new Shapes.GridLine(Canvas.PlotAreaX, Canvas.PlotAreaY + MapToPlotArea(y), Canvas.PlotAreaWidth));

    public double MapToPlotArea(double dataPoint) => (dataPoint - YAxis.Min) / (YAxis.Max - YAxis.Min) * Canvas.PlotAreaHeight;
}
