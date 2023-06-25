using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class XYChart {
    public const string FallbackColor = "#000000";

    public static List<string> DefaultColors { get; set; } = new() {
        // https://coolors.co/550527-688e26-faa613-f44708-a10702
        "#550527", "#688e26", "#faa613", "#f44708", "#a10702"

        // https://coolors.co/264653-2a9d8f-e9c46a-f4a261-e76f51
        // "#264653", "#2a9d8f", "#e9c46a", "#f4a261", "#e76f51"
        
        // https://coolors.co/2274a5-f75c03-f1c40f-d90368-00cc66
        // "#2274a5", "#f75c03", "#f1c40f", "#d90368", "#00cc66"
    };

    public static string GetColor(int index) {
        if (DefaultColors.Any() && index >= 0) {
            return DefaultColors[index % DefaultColors.Count];
        }

        return FallbackColor;
    }

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

    public DataSeries AddDataSeries(string name) => AddDataSeries(name, null);

    public DataSeries AddDataSeries(string name, string? color) {
        var dataSeries = new DataSeries(name, color ?? GetColor(DataSeries.Count));

        dataSeries.AddRange(Enumerable.Range(0, Labels.Count).Select<int, double?>(i => null));
        DataSeries.Add(dataSeries);

        return dataSeries;
    }
}
