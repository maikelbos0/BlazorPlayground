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
    public abstract StackMode StackMode { get; }

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

    public IEnumerable<DataPoint> GetDataPoints() {
        var dataPointTransformer = GetDataPointTransformer();

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => new DataPoint(
                Chart.MapDataIndexToCanvas(value.Index),
                Chart.MapDataPointToCanvas(dataPointTransformer(value.DataPoint!.Value, value.Index)),
                Chart.MapDataValueToPlotArea(value.DataPoint!.Value),
                dataSeriesIndex,
                value.Index
            )))
            .ToList();
    }

    private Func<decimal, int, decimal> GetDataPointTransformer() {
        if (IsStacked) {
            switch (StackMode) {
                case StackMode.Single:
                    var offsets = new decimal[Chart.Labels.Count];

                    return (dataPoint, index) => offsets[index] += dataPoint;
                case StackMode.Split:
                    var negativeOffsets = new decimal[Chart.Labels.Count];
                    var positiveOffsets = new decimal[Chart.Labels.Count];

                    return (dataPoint, index) => (dataPoint < 0 ? negativeOffsets : positiveOffsets)[index] += dataPoint;
                default:
                    throw new NotImplementedException($"Missing stacked transformer implementation for {nameof(StackMode)}{StackMode}");
            }
        }
        else {
            return (dataPoint, index) => dataPoint;
        }
    }
}
