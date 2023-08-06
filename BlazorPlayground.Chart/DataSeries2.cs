using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class DataSeries2 : ComponentBase {
    [CascadingParameter] internal LayerBase2 Layer { get; set; } = null!;
    [Parameter] public string? Name { get; set; }
    [Parameter] public string? Color { get; set; }
    [Parameter] public List<decimal?> DataPoints { get; set; } = new();

    protected override void OnInitialized() {
        if (!Layer.DataSeries.Contains(this)) {
            Layer.DataSeries.Add(this);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(1, nameof(DataSeries2));
}
