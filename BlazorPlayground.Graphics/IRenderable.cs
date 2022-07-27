using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Graphics {
    public interface IRenderable {
        public void BuildRenderTree(RenderTreeBuilder builder);
    }
}
