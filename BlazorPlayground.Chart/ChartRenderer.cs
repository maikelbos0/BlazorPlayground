using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class ChartRenderer : ComponentBase {
    [Parameter]
    [EditorRequired]
    public XYChart? Chart { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        if (Chart != null) {
            builder.OpenElement(1, "svg");
            builder.AddAttribute(2, "class", "chart-main");
            builder.AddAttribute(3, "viewbox", $"0 0 {Chart.Width} {Chart.Height}");
            builder.AddAttribute(4, "width", Chart.Width);
            builder.AddAttribute(5, "height", Chart.Height);
            builder.CloseElement();
        }
    }
}
