using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class DataSeries2 : ComponentBase {
    [CascadingParameter] internal LayerBase2 Layer { get; set; } = null!;

    protected override void OnInitialized() {
        if (!Layer.DataSeries.Contains(this)) {
            Layer.DataSeries.Add(this);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(1, nameof(DataSeries2));
}
