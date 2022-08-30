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

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (Shape != null) {
                builder.OpenElement(1, "g");
                builder.SetKey(Shape);
                builder.AddAttribute(2, "class", IsSelected ? "shape-selected" : IsVirtual ? "shape-virtual" : "");
                builder.AddAttribute(3, "onmousedown", OnMouseDown);
                builder.AddContent(4, Shape.BuildRenderTree);
                builder.CloseElement();
            }
        }
    }
}
