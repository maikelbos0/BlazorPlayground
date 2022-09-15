using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public abstract class DrawableShape : Shape {
        public abstract string ElementName { get; }

        public override void BuildRenderTree(RenderTreeBuilder builder) {
            builder.OpenElement(1, ElementName);

            if (Definition.UseOpacity) {
                builder.AddAttribute(2, "opacity", (Opacity / 100.0).ToString(CultureInfo.InvariantCulture));
            }

            if (this is IShapeWithFill shapeWithFill) {
                builder.AddAttribute(3, "fill", shapeWithFill.GetFill());
            }

            if (Definition.UseStroke) {
                builder.AddAttribute(4, "stroke", Stroke);
            }

            if (Definition.UseStrokeWidth) {
                builder.AddAttribute(5, "stroke-width", StrokeWidth);
            }

            if (Definition.UseStrokeLinecap) {
                builder.AddAttribute(6, "stroke-linecap", StrokeLinecap.ToString().ToLower());
            }

            if (Definition.UseStrokeLinejoin) {
                builder.AddAttribute(7, "stroke-linejoin", StrokeLinejoin.ToString().ToLower());
            }

            builder.AddMultipleAttributes(8, GetAttributes());
            builder.CloseElement();

            builder.OpenElement(9, ElementName);
            builder.AddAttribute(10, "class", "shape-selector");

            if (Definition.UseStrokeLinecap) {
                builder.AddAttribute(11, "stroke-linecap", StrokeLinecap.ToString().ToLower());
            }

            if (Definition.UseStrokeLinejoin) {
                builder.AddAttribute(12, "stroke-linejoin", StrokeLinejoin.ToString().ToLower());
            }

            builder.AddAttribute(13, "stroke-width", Math.Max(StrokeWidth, 12));
            builder.AddMultipleAttributes(14, GetAttributes());
            builder.CloseElement();
        }

        public abstract ShapeAttributeCollection GetAttributes();

        public override XElement CreateSvgElement() => new(ElementName, CreateSvgAttributes());

        private IEnumerable<XAttribute> CreateSvgAttributes() {
            if (Definition.UseOpacity) {
                yield return new XAttribute("opacity", Opacity / 100.0);
            }

            if (this is IShapeWithFill shapeWithFill) {
                yield return new XAttribute("fill", shapeWithFill.GetFill());
            }

            if (Definition.UseStroke) {
                yield return new XAttribute("stroke", Stroke);
            }

            if (Definition.UseStrokeWidth) {
                yield return new XAttribute("stroke-width", StrokeWidth);
            }

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
