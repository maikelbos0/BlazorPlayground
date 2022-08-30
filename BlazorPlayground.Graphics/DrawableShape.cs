using Microsoft.AspNetCore.Components.Rendering;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public abstract class DrawableShape : Shape {
        public abstract string ElementName { get; }

        public override void BuildRenderTree(RenderTreeBuilder builder) {            
            builder.OpenElement(1, ElementName);

            if (Definition.UseFill) {
                builder.AddAttribute(2, "fill", Fill);
            }

            builder.AddAttribute(3, "stroke", Stroke);
            builder.AddAttribute(4, "stroke-width", StrokeWidth);

            if (Definition.UseStrokeLinecap) {
                builder.AddAttribute(5, "stroke-linecap", StrokeLinecap.ToString().ToLower());
            }

            if (Definition.UseStrokeLinejoin) {
                builder.AddAttribute(6, "stroke-linejoin", StrokeLinejoin.ToString().ToLower());
            }

            builder.AddMultipleAttributes(7, GetAttributes());
            builder.CloseElement();

            builder.OpenElement(8, ElementName);
            builder.AddAttribute(9, "class", "shape-selector");

            if (Definition.UseStrokeLinecap) {
                builder.AddAttribute(10, "stroke-linecap", StrokeLinecap.ToString().ToLower());
            }

            if (Definition.UseStrokeLinejoin) {
                builder.AddAttribute(11, "stroke-linejoin", StrokeLinejoin.ToString().ToLower());
            }

            builder.AddAttribute(12, "stroke-width", Math.Max(StrokeWidth, 12));
            builder.AddMultipleAttributes(13, GetAttributes());
            builder.CloseElement();
        }

        public abstract ShapeAttributeCollection GetAttributes();
        
        public override XElement CreateSvgElement() => new(ElementName, CreateSvgAttributes());

        private IEnumerable<XAttribute> CreateSvgAttributes() {
            if (Definition.UseFill) {
                yield return new XAttribute("fill", Fill);
            }

            yield return new XAttribute("stroke", Stroke);
            yield return new XAttribute("stroke-width", StrokeWidth);

            if (Definition.UseStrokeLinecap) {
                yield return new XAttribute("stroke-linecap", StrokeLinecap.ToString().ToLower());
            }

            if (Definition.UseStrokeLinejoin) {
                yield return new XAttribute("stroke-linejoin", StrokeLinejoin.ToString().ToLower());
            }

            foreach (var attribute in GetAttributes()) {
                yield return new XAttribute(attribute.Key, attribute.Value);
            }

            yield return new XAttribute("data-shape-type", GetType().Name);

            for (var i = 0; i < Anchors.Count; i++) {
                var point = Anchors[i].Get(this);

                yield return new XAttribute($"data-shape-anchor-{i}", FormattableString.Invariant($"{point.X},{point.Y}"));
            }

            if (Definition.UseSides) {
                yield return new XAttribute("data-shape-sides", Sides);
            }
        }
    }
}
