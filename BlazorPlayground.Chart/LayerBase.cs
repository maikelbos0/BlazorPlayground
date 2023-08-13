using BlazorPlayground.Chart.Shapes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public abstract class LayerBase : ComponentBase, IDisposable {
    public static bool DefaultIsStacked { get; set; } = false;

    [CascadingParameter] internal XYChart Chart { get; set; } = null!;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool IsStacked { get; set; } = DefaultIsStacked;
    internal List<DataSeries> DataSeries { get; set; } = new();
    public abstract StackMode StackMode { get; }
    public abstract DataPointSpacingMode DefaultDataPointSpacingMode { get; }

    protected override void OnInitialized() => Chart.AddLayer(this);

    public void Dispose() => Chart.RemoveLayer(this);

    internal void AddDataSeries(DataSeries dataSeries) {
        if (!DataSeries.Contains(dataSeries)) {
            DataSeries.Add(dataSeries);
        }

        Chart.HandleStateChange();
    }

    internal void RemoveDataSeries(DataSeries dataSeries) {
        DataSeries.Remove(dataSeries);
        Chart.HandleStateChange();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<LayerBase>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }

    public abstract IEnumerable<ShapeBase> GetDataSeriesShapes();

    public IEnumerable<CanvasDataPoint> GetCanvasDataPoints() {
        var dataPointTransformer = GetDataPointTransformer();

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries.DataPoints
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => new CanvasDataPoint(
                Chart.MapDataIndexToCanvas(value.Index),
                Chart.MapDataPointToCanvas(dataPointTransformer(value.DataPoint!.Value, value.Index)),
                Chart.MapDataValueToPlotArea(value.DataPoint!.Value),
                dataSeriesIndex,
                value.Index
            )))
            .ToList();
    }

    public IEnumerable<decimal> GetScaleDataPoints() {
        var dataPointTransformer = GetDataPointTransformer();

        return DataSeries.SelectMany(dataSeries => dataSeries.DataPoints
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => dataPointTransformer(value.DataPoint!.Value, value.Index)));
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
