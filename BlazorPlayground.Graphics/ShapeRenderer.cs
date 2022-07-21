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
                builder.AddAttribute(2, "stroke-linecap", Shape.StrokeLinecap.ToString().ToLower());
                builder.AddAttribute(3, "stroke-linejoin", Shape.StrokeLinejoin.ToString().ToLower());
                builder.SetKey(Shape);

                builder.OpenElement(4, Shape.ElementName);
                builder.AddAttribute(5, "class", IsSelected ? "shape-selected" : IsVirtual ? "shape-virtual" : "");
                builder.AddAttribute(6, "fill", Shape.Fill);
                builder.AddAttribute(7, "stroke", Shape.Stroke);
                builder.AddAttribute(8, "stroke-width", Shape.StrokeWidth);
                builder.AddMultipleAttributes(9, Shape.GetAttributes());
                builder.CloseElement();

                builder.OpenElement(10, Shape.ElementName);
                builder.AddAttribute(11, "class", "shape-selector");
                builder.AddAttribute(12, "onmousedown", OnMouseDown);
                builder.AddAttribute(13, "onmouseup", OnMouseUp);
                builder.AddAttribute(14, "stroke-width", Math.Max(Shape.StrokeWidth, 12));
                builder.AddMultipleAttributes(15, Shape.GetAttributes());
                builder.CloseElement();

                builder.CloseElement();
            }
        }
    }
}
