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
                builder.OpenRegion(1);
                builder.OpenElement(1, Shape.ElementName);
                builder.SetKey(Shape);
                builder.AddAttribute(4, "class", IsSelected ? "shape-selected" : IsVirtual ? "shape-virtual" : "");
                builder.AddAttribute(5, "fill", Shape.Fill);
                builder.AddAttribute(6, "stroke", Shape.Stroke);
                builder.AddAttribute(7, "stroke-width", Shape.StrokeWidth);

                var sequence = 8;

                foreach (var attribute in Shape.GetAttributes()) {
                    builder.AddAttribute(sequence++, attribute.Name, attribute.Value);
                }

                builder.CloseElement();
                builder.CloseRegion();

                builder.OpenRegion(2);
                builder.OpenElement(1, Shape.ElementName);
                builder.AddAttribute(2, "onmousedown", OnMouseDown);
                builder.AddAttribute(3, "onmouseup", OnMouseUp);
                builder.AddAttribute(4, "stroke-width", Math.Max(Shape.StrokeWidth, 10));
                builder.AddAttribute(5, "class", "shape-selector");

                sequence = 6;

                foreach (var attribute in Shape.GetAttributes()) {
                    builder.AddAttribute(sequence++, attribute.Name, attribute.Value);
                }

                builder.CloseElement();
                builder.CloseRegion();
            }
        }
    }
}
