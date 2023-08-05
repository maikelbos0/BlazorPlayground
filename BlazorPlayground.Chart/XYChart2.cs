using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class XYChart2 : ComponentBase {
    public Canvas2 Canvas { get; set; } = new();
    public PlotArea2 PlotArea { get; set; } = new();
    public List<LayerBase2> Layers { get; set; } = new();

    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<XYChart2>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }
}
