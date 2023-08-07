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

    public decimal MapDataPointToCanvas(decimal dataPoint) => Canvas.PlotAreaY + MapDataValueToPlotArea(PlotArea.Max - dataPoint);

    public decimal MapDataValueToPlotArea(decimal dataPoint) => dataPoint / (PlotArea.Max - PlotArea.Min) * Canvas.PlotAreaHeight;

    public decimal MapDataIndexToCanvas(int index) => Canvas.PlotAreaX + (index + 0.5M) * DataPointWidth;
}
