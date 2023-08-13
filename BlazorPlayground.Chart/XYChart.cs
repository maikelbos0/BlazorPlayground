using BlazorPlayground.Chart.Shapes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class XYChart : ComponentBase {
    public static DataPointSpacingMode DefaultDataPointSpacingMode { get; set; } = DataPointSpacingMode.Auto;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public List<string> Labels { get; set; } = new();
    [Parameter] public DataPointSpacingMode DataPointSpacingMode { get; set; } = DefaultDataPointSpacingMode;
    internal Canvas Canvas { get; set; } = new();
    internal PlotArea PlotArea { get; set; } = new();
    internal List<LayerBase> Layers { get; set; } = new();
    internal Action? StateHasChangedHandler { get; init; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<XYChart>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();

        builder.OpenElement(1, "svg");
        builder.AddAttribute(2, "class", "chart-main");
        builder.AddAttribute(3, "viewbox", $"0 0 {Canvas.Width} {Canvas.Height}");
        builder.AddAttribute(4, "width", Canvas.Width);
        builder.AddAttribute(5, "height", Canvas.Height);

        foreach (var shape in GetShapes()) {
            builder.OpenElement(6, shape.ElementName);
            builder.SetKey(shape.Key);
            builder.AddAttribute(7, "class", shape.CssClass);
            builder.AddMultipleAttributes(8, shape.GetAttributes());
            builder.AddContent(9, shape.GetContent());
            builder.CloseElement();
        }

        builder.CloseElement();
    }

    internal void SetCanvas(Canvas canvas) {
        Canvas = canvas;
        HandleStateChange();
    }

    internal void ResetCanvas() {
        Canvas = new();
        HandleStateChange();
    }

    internal void SetPlotArea(PlotArea plotArea) {
        PlotArea = plotArea;
        HandleStateChange();
    }

    internal void ResetPlotArea() {
        PlotArea = new();
        HandleStateChange();
    }

    internal void AddLayer(LayerBase layer) {
        if (!Layers.Contains(layer)) {
            Layers.Add(layer);
        }

        HandleStateChange();
    }

    internal void RemoveLayer(LayerBase layer) {
        Layers.Remove(layer);
        HandleStateChange();
    }

    internal void HandleStateChange() => (StateHasChangedHandler ?? StateHasChanged)();

    public IEnumerable<ShapeBase> GetShapes() {
        PlotArea.AutoScale(Layers.SelectMany(layer => layer.GetScaleDataPoints()));

        foreach (var shape in GetGridLineShapes()) {
            yield return shape;
        }

        foreach (var shape in GetDataSeriesShapes()) {
            yield return shape;
        }

        yield return Canvas.GetPlotAreaShape();

        foreach (var shape in GetYAxisLabelShapes()) {
            yield return shape;
        }

        if (GetYAxisMultiplierShape() is ShapeBase multiplierShape) {
            yield return multiplierShape;
        }

        foreach (var shape in GetXAxisLabelShapes()) {
            yield return shape;
        }
    }

    public IEnumerable<GridLineShape> GetGridLineShapes()
        => PlotArea.GetGridLineDataPoints().Select((dataPoint, index) => new GridLineShape(Canvas.PlotAreaX, MapDataPointToCanvas(dataPoint), Canvas.PlotAreaWidth, dataPoint, index));

    public IEnumerable<YAxisLabelShape> GetYAxisLabelShapes()
        => PlotArea.GetGridLineDataPoints().Select((dataPoint, index) => new YAxisLabelShape(Canvas.PlotAreaX - Canvas.YAxisLabelClearance, MapDataPointToCanvas(dataPoint), (dataPoint / PlotArea.Multiplier).ToString(Canvas.YAxisLabelFormat), index));

    public YAxisMultiplierShape? GetYAxisMultiplierShape()
        => PlotArea.Multiplier == 1M ? null : new YAxisMultiplierShape(Canvas.Padding, Canvas.PlotAreaY + Canvas.PlotAreaHeight / 2M, PlotArea.Multiplier.ToString(Canvas.YAxisMultiplierFormat));

    public IEnumerable<XAxisLabelShape> GetXAxisLabelShapes()
        => Labels.Select((label, index) => new XAxisLabelShape(MapDataIndexToCanvas(index), Canvas.PlotAreaY + Canvas.PlotAreaHeight + Canvas.XAxisLabelClearance, label, index));

    public IEnumerable<ShapeBase> GetDataSeriesShapes()
        => Layers.SelectMany(layer => layer.GetDataSeriesShapes());

    public decimal MapDataPointToCanvas(decimal dataPoint) => Canvas.PlotAreaY + MapDataValueToPlotArea(PlotArea.Max - dataPoint);

    public decimal MapDataValueToPlotArea(decimal dataPoint) => dataPoint / (PlotArea.Max - PlotArea.Min) * Canvas.PlotAreaHeight;

    public DataPointSpacingMode GetDataPointSpacingMode() => DataPointSpacingMode switch {
        DataPointSpacingMode.Auto => Layers.Select(layer => layer.DefaultDataPointSpacingMode).DefaultIfEmpty(DataPointSpacingMode.EdgeToEdge).Max(),
        _ => DataPointSpacingMode
    };

    public decimal GetDataPointWidth() => GetDataPointSpacingMode() switch {
        DataPointSpacingMode.EdgeToEdge => ((decimal)Canvas.PlotAreaWidth) / Math.Max(1, Labels.Count - 1),
        DataPointSpacingMode.Center => ((decimal)Canvas.PlotAreaWidth) / Math.Max(1, Labels.Count),
        _ => throw new NotImplementedException($"No implementation found for {nameof(DataPointSpacingMode)} '{DataPointSpacingMode}'.")
    };

    public decimal MapDataIndexToCanvas(int index) => GetDataPointSpacingMode() switch {
        DataPointSpacingMode.EdgeToEdge => Canvas.PlotAreaX + index * GetDataPointWidth(),
        DataPointSpacingMode.Center => Canvas.PlotAreaX + (index + 0.5M) * GetDataPointWidth(),
        _ => throw new NotImplementedException($"No implementation found for {nameof(DataPointSpacingMode)} '{DataPointSpacingMode}'.")
    };
}
