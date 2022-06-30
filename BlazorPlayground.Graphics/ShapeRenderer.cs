using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Graphics {
    public class ShapeRenderer : ComponentBase {
        [Parameter]
        public IShape? Shape { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (Shape != null) {
                builder.OpenElement(1, GetElementName(Shape.GetSeriesType()));
                builder.AddAttribute(2, "style", "stroke: #000; stroke-width: 2; fill: transparent;");
                builder.AddAttribute(3, "points", string.Join(" ", Shape.GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))));
                builder.CloseElement();
            }
        }

        public string GetElementName(RenderType renderType)
            => renderType switch {
                RenderType.Polyline => "polyline",
                RenderType.Polygon => "polygon",
                _ => throw new NotImplementedException($"No implementation found for {nameof(RenderType)} '{renderType}'")
            };
}
}
