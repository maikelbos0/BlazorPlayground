using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class LayerBase2 : ComponentBase {
    [CascadingParameter] internal XYChart2 Chart { get; set; } = null!;

    [Parameter] public RenderFragment? ChildContent { get; set; }

    public List<DataSeries2> DataSeries { get; set; } = new();

    protected override void OnInitialized() {
        if (!Chart.Layers.Contains(this)) {
            Chart.Layers.Add(this);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.AddContent(1, nameof(LayerBase2));
        builder.OpenComponent<CascadingValue<LayerBase2>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }
}
