using BlazorPlayground.Chart.Shapes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class XYChart2 : ComponentBase {
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public List<string> Labels { get; set; } = new();
    public Canvas2 Canvas { get; set; } = new();
    public PlotArea2 PlotArea { get; set; } = new();
    public List<LayerBase2> Layers { get; set; } = new();    
    public decimal DataPointWidth => ((decimal)Canvas.PlotAreaWidth) / Labels.Count;

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<XYChart2>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }

    public IEnumerable<ShapeBase> GetShapes() {
        PlotArea.AutoScale(GetScaleDataPoints());

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

    public IEnumerable<decimal> GetScaleDataPoints() {
        var dataPoints = new List<decimal>();

        foreach (var layer in Layers) {
            // TODO add take label count here
            dataPoints.AddRange(layer.DataSeries.SelectMany(dataSeries => dataSeries.DataPoints.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value)));

            if (layer.IsStacked) {
                for (var i = 0; i < Labels.Count; i++) {
                    // TODO take stack mode into account
                    if (layer.DataSeries.Any(dataSeries => dataSeries.DataPoints.Count > i && dataSeries.DataPoints[i] < 0)) {
                        dataPoints.Add(layer.DataSeries.Where(dataSeries => dataSeries.DataPoints.Count > i && dataSeries.DataPoints[i] < 0).Sum(dataSeries => dataSeries.DataPoints[i]!.Value));
                    }

                    if (layer.DataSeries.Any(dataSeries => dataSeries.DataPoints.Count > i && dataSeries.DataPoints[i] > 0)) {
                        dataPoints.Add(layer.DataSeries.Where(dataSeries => dataSeries.DataPoints.Count > i && dataSeries.DataPoints[i] > 0).Sum(dataSeries => dataSeries.DataPoints[i]!.Value));
                    }
                }
            }
        }

        return dataPoints;
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

    public decimal MapDataIndexToCanvas(int index) => Canvas.PlotAreaX + (index + 0.5M) * DataPointWidth;
}
