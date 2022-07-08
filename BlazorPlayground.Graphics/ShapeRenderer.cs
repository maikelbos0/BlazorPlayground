using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Graphics {
    public class ShapeRenderer : ComponentBase {
        [Parameter]
        public Shape? Shape { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (Shape != null) {
                builder.OpenElement(1, GetElementName(Shape.RenderType));
                builder.AddAttribute(2, "class", GetClassName(Shape.RenderMode));
                builder.AddAttribute(3, "points", string.Join(" ", Shape.GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))));
                builder.CloseElement();
            }
        }

        public string GetClassName(ShapeRenderMode renderMode)
            => renderMode switch {
                ShapeRenderMode.Default => "shape",
                ShapeRenderMode.New => "shape shape-new",
                _ => throw new NotImplementedException($"No implementation found for {nameof(ShapeRenderMode)} '{renderMode}'")
            };

        public string GetElementName(ShapeRenderType renderType)
            => renderType switch {
                ShapeRenderType.Polyline => "polyline",
                ShapeRenderType.Polygon => "polygon",
                _ => throw new NotImplementedException($"No implementation found for {nameof(ShapeRenderType)} '{renderType}'")
            };
}
}
