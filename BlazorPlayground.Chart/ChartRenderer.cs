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
            builder.AddAttribute(3, "viewbox", $"0 0 {Chart.Canvas.Width} {Chart.Canvas.Height}");
            builder.AddAttribute(4, "width", Chart.Canvas.Width);
            builder.AddAttribute(5, "height", Chart.Canvas.Height);

            foreach (var shape in Chart.GetShapes()) {
                builder.OpenElement(6, shape.ElementName);
                builder.SetKey(shape.GetKey());
                builder.AddAttribute(7, "class", shape.CssClass);
                builder.AddMultipleAttributes(8, shape.GetAttributes());
                builder.AddContent(9, shape.GetContent());
                builder.CloseElement();
            }

            builder.CloseElement();
        }
    }
}
