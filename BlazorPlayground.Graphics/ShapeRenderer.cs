using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPlayground.Graphics {
    public class ShapeRenderer : ComponentBase {
        [Parameter]
        public Shape? Shape { get; set; }

        [Parameter]
        public bool IsVirtual { get; set; } = false;

        [Parameter]
        public bool IsSelected { get; set; } = false;

        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseDown { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseUp { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (Shape != null) {
                builder.OpenElement(1, "g");
                builder.SetKey(Shape);

                builder.OpenElement(2, Shape.ElementName);
                builder.AddAttribute(3, "class", IsSelected ? "shape-selected" : IsVirtual ? "shape-virtual" : "");
                builder.AddAttribute(4, "fill", Shape.Fill);
                builder.AddAttribute(5, "stroke", Shape.Stroke);
                builder.AddAttribute(6, "stroke-width", Shape.StrokeWidth);
                builder.AddMultipleAttributes(7, Shape.GetAttributes());
                builder.CloseElement();

                builder.OpenElement(8, Shape.ElementName);
                builder.AddAttribute(9, "class", "shape-selector");
                builder.AddAttribute(10, "onmousedown", OnMouseDown);
                builder.AddAttribute(11, "onmouseup", OnMouseUp);
                builder.AddAttribute(12, "stroke-width", Math.Max(Shape.StrokeWidth, 10));
                builder.AddMultipleAttributes(13, Shape.GetAttributes());
                builder.CloseElement();

                builder.CloseElement();
            }
        }
    }
}
