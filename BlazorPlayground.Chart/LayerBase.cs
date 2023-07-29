using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public abstract class LayerBase {
    public const string FallbackColor = "#000000";

    public static bool DefaultIsStacked { get; set; } = false;
    public static List<string> DefaultColors { get; set; } = new() {
        // https://coolors.co/550527-688e26-faa613-f44708-a10702
        // "#550527", "#688e26", "#faa613", "#f44708", "#a10702"

        // https://coolors.co/264653-2a9d8f-e9c46a-f4a261-e76f51
        // "#264653", "#2a9d8f", "#e9c46a", "#f4a261", "#e76f51"
        
        // https://coolors.co/2274a5-f75c03-f1c40f-d90368-00cc66
        "#2274a5", "#f75c03", "#f1c40f", "#d90368", "#00cc66"
    };

    public static string GetColor(int index) {
        if (DefaultColors.Any() && index >= 0) {
            return DefaultColors[index % DefaultColors.Count];
        }

        return FallbackColor;
    }

    public XYChart Chart { get; }
    public bool IsStacked { get; set; } = DefaultIsStacked;
    public List<DataSeries> DataSeries { get; set; } = new();

    public LayerBase(XYChart chart) {
        Chart = chart;
    }

    public DataSeries AddDataSeries(string name) => AddDataSeries(name, null);

    public DataSeries AddDataSeries(string name, string? color) {
        var dataSeries = new DataSeries(name, color ?? GetColor(DataSeries.Count));

        dataSeries.AddRange(Enumerable.Range(0, Chart.Labels.Count).Select<int, decimal?>(i => null));
        DataSeries.Add(dataSeries);

        return dataSeries;
    }

    public abstract IEnumerable<ShapeBase> GetDataSeriesShapes();

    public IEnumerable<DataSeriesPoint> GetDataSeriesPoints() {
        Func<decimal, int, decimal> transformer = (dataPoint, index) => dataPoint;

        if (IsStacked) {
            var negativeOffsets = new decimal[Chart.Labels.Count];
            var positiveOffsets = new decimal[Chart.Labels.Count];

            transformer = (dataPoint, index) => (dataPoint < 0 ? negativeOffsets : positiveOffsets)[index] += dataPoint;
        }

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => new DataSeriesPoint(
                Chart.MapDataIndexToCanvas(value.Index),
                Chart.MapDataPointToCanvas(transformer(value.DataPoint!.Value, value.Index)),
                Chart.MapDataValueToPlotArea(value.DataPoint!.Value),
                dataSeries.Color,
                dataSeriesIndex,
                value.Index
            )))
            .ToList();
    }
}
