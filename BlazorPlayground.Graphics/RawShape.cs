using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public class RawShape : Shape, IRenderable {
        private readonly XElement element;

        public RawShape(XElement element) {
            this.element = element;
        }

        public override string ElementName => element.Name.ToString();

        public override IReadOnlyList<Anchor> Anchors => new ReadOnlyCollection<Anchor>(Array.Empty<Anchor>());

        protected override Shape CreateClone() => new RawShape(new XElement(element));

        // TODO refactor this; GetAttributes is not a property of a renderable shape at all
        public override ShapeAttributeCollection GetAttributes() => new();

        public override XElement CreateSvgElement() => new(element);

        public void BuildRenderTree(RenderTreeBuilder builder) {
            builder.AddContent(1, new MarkupString(element.ToString()));
        }
    }
}
