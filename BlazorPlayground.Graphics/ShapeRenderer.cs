﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPlayground.Graphics {
    public class ShapeRenderer : ComponentBase {
        [Parameter]
        public Shape? Shape { get; set; }

        [Parameter]
        public bool IsNew { get; set; } = false;

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (Shape != null) {
                builder.OpenElement(1, GetElementName(Shape.RenderType));
                builder.AddAttribute(2, "onclick", OnClick);
                builder.AddAttribute(3, "class", $"shape {(IsNew ? "shape-new" : "")} {(Shape.IsSelected ? "shape-selected" : "")}");
                builder.AddAttribute(4, "points", string.Join(" ", Shape.GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))));
                builder.CloseElement();
            }
        }

        public string GetElementName(ShapeRenderType renderType)
            => renderType switch {
                ShapeRenderType.Polyline => "polyline",
                ShapeRenderType.Polygon => "polygon",
                _ => throw new NotImplementedException($"No implementation found for {nameof(ShapeRenderType)} '{renderType}'")
            };
}
}
