using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class PlotArea2 : ComponentBase {
    [CascadingParameter] internal XYChart2 Chart { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public AutoScaleSettings2 AutoScaleSettings { get; set; } = new();
    protected override void OnInitialized() {
        Chart.PlotArea = this;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.AddContent(1, nameof(PlotArea2));
        builder.OpenComponent<CascadingValue<PlotArea2>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }
}
