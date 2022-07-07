using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Graphics {
    public class ShapeRenderer : ComponentBase {
        [Parameter]
        public IShape? Shape { get; set; }

        [Parameter]
        public ShapeRenderMode RenderMode { get; set; } = ShapeRenderMode.None;

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (Shape != null) {
                builder.OpenElement(1, GetElementName(Shape.GetSeriesType()));
                builder.AddAttribute(2, "class", GetClassName());
                builder.AddAttribute(3, "points", string.Join(" ", Shape.GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))));
                builder.CloseElement();
            }
        }

        public string GetClassName()
            => RenderMode switch {
                ShapeRenderMode.None => "shape",
                ShapeRenderMode.New => "shape shape-new",
                _ => throw new NotImplementedException($"No implementation found for {nameof(ShapeRenderMode)} '{RenderMode}'")
            };

        public string GetElementName(ShapeRenderType renderType)
            => renderType switch {
                ShapeRenderType.Polyline => "polyline",
                ShapeRenderType.Polygon => "polygon",
                _ => throw new NotImplementedException($"No implementation found for {nameof(ShapeRenderType)} '{renderType}'")
            };
}
}
