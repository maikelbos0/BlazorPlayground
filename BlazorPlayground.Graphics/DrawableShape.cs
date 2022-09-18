using Microsoft.AspNetCore.Components.Rendering;
using System.Globalization;
using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public abstract class DrawableShape : Shape {
        public abstract string ElementName { get; }

        public override void BuildRenderTree(RenderTreeBuilder builder) {
            builder.OpenElement(1, ElementName);

            if (this is IShapeWithOpacity shapeWithOpacity) {
                builder.AddAttribute(2, "opacity", (shapeWithOpacity.GetOpacity() / 100.0).ToString(CultureInfo.InvariantCulture));
            }

            if (this is IShapeWithFill shapeWithFill) {
                builder.AddAttribute(3, "fill", shapeWithFill.GetFill());
                builder.AddAttribute(4, "fill-opacity", (shapeWithFill.GetFillOpacity() / 100.0).ToString(CultureInfo.InvariantCulture));
            }

            if (this is IShapeWithStroke shapeWithStroke) {
                builder.AddAttribute(5, "stroke", shapeWithStroke.GetStroke());
                builder.AddAttribute(6, "stroke-width", shapeWithStroke.GetStrokeWidth());
                builder.AddAttribute(7, "stroke-opacity", (shapeWithStroke.GetStrokeOpacity() / 100.0).ToString(CultureInfo.InvariantCulture));
            }

            if (this is IShapeWithStrokeLinecap shapeWithStrokeLinecap) {
                builder.AddAttribute(8, "stroke-linecap", shapeWithStrokeLinecap.GetStrokeLinecap().ToString().ToLower());
            }

            if (this is IShapeWithStrokeLinejoin shapeWithStrokeLinejoin) {
                builder.AddAttribute(9, "stroke-linejoin", shapeWithStrokeLinejoin.GetStrokeLinejoin().ToString().ToLower());
            }
            
            builder.AddMultipleAttributes(10, GetAttributes());
            builder.CloseElement();

            builder.OpenElement(11, ElementName);
            builder.AddAttribute(12, "class", "shape-selector");
            builder.AddAttribute(13, "stroke-width", 12);
            builder.AddMultipleAttributes(14, GetAttributes());
            builder.CloseElement();
        }

        public abstract ShapeAttributeCollection GetAttributes();

        public override XElement CreateSvgElement() => new(ElementName, CreateSvgAttributes());

        private IEnumerable<XAttribute> CreateSvgAttributes() {
            if (this is IShapeWithOpacity shapeWithOpacity) {
                yield return new XAttribute("opacity", shapeWithOpacity.GetOpacity() / 100.0);
            }

            if (this is IShapeWithFill shapeWithFill) {
                yield return new XAttribute("fill", shapeWithFill.GetFill());
                yield return new XAttribute("fill-opacity", shapeWithFill.GetFillOpacity() / 100.0);
            }

            if (this is IShapeWithStroke shapeWithStroke) {
                yield return new XAttribute("stroke", shapeWithStroke.GetStroke());
                yield return new XAttribute("stroke-width", shapeWithStroke.GetStrokeWidth());
                yield return new XAttribute("stroke-opacity", shapeWithStroke.GetStrokeOpacity() / 100.0);
            }

            if (this is IShapeWithStrokeLinecap shapeWithStrokeLinecap) {
                yield return new XAttribute("stroke-linecap", shapeWithStrokeLinecap.GetStrokeLinecap().ToString().ToLower());
            }

            if (this is IShapeWithStrokeLinejoin shapeWithStrokeLinejoin) {
                yield return new XAttribute("stroke-linejoin", shapeWithStrokeLinejoin.GetStrokeLinejoin().ToString().ToLower());
            }

            foreach (var attribute in GetAttributes()) {
                yield return new XAttribute(attribute.Key, attribute.Value);
            }

            yield return new XAttribute("data-shape-type", GetType().Name);

            for (var i = 0; i < Anchors.Count; i++) {
                var point = Anchors[i].Get(this);

                yield return new XAttribute($"data-shape-anchor-{i}", FormattableString.Invariant($"{point.X},{point.Y}"));
            }

            if (this is IShapeWithSides shapeWithSides) {
                yield return new XAttribute("data-shape-sides", shapeWithSides.GetSides());
            }
        }
    }
}
