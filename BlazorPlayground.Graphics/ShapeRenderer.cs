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
                builder.OpenElement(1, Shape.ElementName);
                builder.SetKey(Shape);
                builder.AddAttribute(2, "onmousedown", OnMouseDown);
                builder.AddAttribute(3, "onmouseup", OnMouseUp);
                builder.AddAttribute(4, "class", $"shape {(IsSelected ? "shape-selected" : IsVirtual ? "shape-virtual" : "")}");
                builder.AddAttribute(5, "fill", Shape.FillColor);
                builder.AddAttribute(6, "stroke", Shape.StrokeColor);
                builder.AddAttribute(7, "stroke-width", Shape.StrokeWidth);

                var sequence = 8;

                foreach (var attribute in Shape.GetAttributes()) {
                    builder.AddAttribute(sequence++, attribute.Name, attribute.Value);
                }

                builder.CloseElement();
            }
        }
    }
}
