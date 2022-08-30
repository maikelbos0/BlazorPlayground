using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    // TODO make raw shape selectable with display, delete and move forward/front/backward/back
    public class RawShape : Shape {
        private readonly XElement element;

        public RawShape(XElement element) {
            this.element = element;
        }

        public override IReadOnlyList<Anchor> Anchors => new ReadOnlyCollection<Anchor>(Array.Empty<Anchor>());

        protected override Shape CreateClone() => new RawShape(new XElement(element));

        public override XElement CreateSvgElement() => new(element);

        public override void BuildRenderTree(RenderTreeBuilder builder, ShapeRenderer renderer) {
            builder.AddContent(1, new MarkupString(element.ToString()));
        }
    }
}
