using Microsoft.AspNetCore.Components.Rendering;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public abstract class DrawableShape : Shape {
        public abstract string ElementName { get; }

        // TODO remove renderer here, move renderer attributes properties back to renderer BuildRenderTree (as group?)
        public override void BuildRenderTree(RenderTreeBuilder builder, ShapeRenderer renderer) {
            builder.OpenElement(1, "g");

            if (Definition.UseStrokeLinecap) {
                builder.AddAttribute(2, "stroke-linecap", StrokeLinecap.ToString().ToLower());
            }

            if (Definition.UseStrokeLinejoin) {
                builder.AddAttribute(3, "stroke-linejoin", StrokeLinejoin.ToString().ToLower());
            }

            builder.SetKey(this);

            builder.OpenElement(4, ElementName);
            builder.AddAttribute(5, "class", renderer.IsSelected ? "shape-selected" : renderer.IsVirtual ? "shape-virtual" : "");

            if (Definition.UseFill) {
                builder.AddAttribute(6, "fill", Fill);
            }

            builder.AddAttribute(7, "stroke", Stroke);
            builder.AddAttribute(8, "stroke-width", StrokeWidth);
            builder.AddMultipleAttributes(9, GetAttributes());
            builder.CloseElement();

            builder.OpenElement(10, ElementName);
            builder.AddAttribute(11, "class", "shape-selector");
            builder.AddAttribute(12, "onmousedown", renderer.OnMouseDown);
            builder.AddAttribute(13, "stroke-width", Math.Max(StrokeWidth, 12));
            builder.AddMultipleAttributes(14, GetAttributes());
            builder.CloseElement();

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
