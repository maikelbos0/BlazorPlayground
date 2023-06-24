using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class XYChart {
    public Canvas Canvas = new();
    public PlotArea PlotArea { get; set; } = new();
    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();

    public void AutoScale() => PlotArea.AutoScale(DataSeries.SelectMany(dataSeries => dataSeries.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value)));

    public IEnumerable<ShapeBase> GetShapes() {
        yield return Canvas.GetPlotAreaShape();

        foreach (var gridLine in GetGridLineShapes()) {
            yield return gridLine;
        }
    }

    public IEnumerable<GridLineShape> GetGridLineShapes() => PlotArea.GetGridLines().Select(y => new GridLineShape(Canvas.PlotAreaX, Canvas.PlotAreaY + MapToPlotArea(y), Canvas.PlotAreaWidth));

    public double MapToPlotArea(double dataPoint) => (dataPoint - PlotArea.Min) / (PlotArea.Max - PlotArea.Min) * Canvas.PlotAreaHeight;
}
