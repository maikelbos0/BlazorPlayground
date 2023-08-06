using BlazorPlayground.Chart.Shapes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public abstract class LayerBase2 : ComponentBase {
    [CascadingParameter] internal XYChart2 Chart { get; set; } = null!;

    public static bool DefaultIsStacked { get; set; } = false;
    
    [CascadingParameter] internal XYChart2 Chart { get; set; } = null!;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    public bool IsStacked { get; set; } = DefaultIsStacked;
    public List<DataSeries2> DataSeries { get; set; } = new();
    public abstract StackMode StackMode { get; }

    protected override void OnInitialized() {
        if (!Chart.Layers.Contains(this)) {
            Chart.Layers.Add(this);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.AddContent(1, GetType().Name);
        builder.OpenComponent<CascadingValue<LayerBase2>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }

    public abstract IEnumerable<ShapeBase> GetDataSeriesShapes();

    public IEnumerable<DataPoint> GetDataPoints() {
        var dataPointTransformer = GetDataPointTransformer();

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries.DataPoints
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
