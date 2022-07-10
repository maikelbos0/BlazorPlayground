using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using System.Reflection;

namespace BlazorPlayground.Graphics {
    public class ShapeRenderer : ComponentBase {
        private readonly static Dictionary<Type, PropertyInfo[]> shapePointProperties = new Dictionary<Type, PropertyInfo[]>();

        private static PropertyInfo[] GetShapePointProperties(Type shapeType) {
            if (!shapePointProperties.TryGetValue(shapeType, out var properties)) {
                properties = shapeType.GetProperties().Where(p => p.PropertyType == typeof(Point) && p.CanWrite && p.CanRead).ToArray();

                shapePointProperties[shapeType] = properties;
            }

            return properties;
        }

        [Parameter]
        public Shape? Shape { get; set; }

        [Parameter]
        public bool IsNew { get; set; } = false;

        [Parameter]
        public EventCallback<MouseEventArgs> OnClick { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (Shape != null) {
                builder.OpenElement(1, GetElementName(Shape.RenderType));
                builder.SetKey(Shape);
                builder.AddAttribute(2, "onclick", OnClick);
                builder.AddAttribute(3, "class", $"shape {(IsNew ? "shape-new" : "")} {(Shape.IsSelected ? "shape-selected" : "")}");
                builder.AddAttribute(4, "points", string.Join(" ", Shape.GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))));
                builder.CloseElement();

                if (Shape.IsSelected) {
                    foreach (var property in GetShapePointProperties(Shape.GetType())) {
                        var point = property.GetValue(Shape) as Point;

                        if (point != null) {
                            builder.OpenElement(1, "rect");
                            builder.SetKey(point);
                            builder.AddAttribute(3, "class", "shape-point");
                            builder.AddAttribute(4, "x", point.X - 5);
                            builder.AddAttribute(5, "y", point.Y - 5);
                            builder.AddAttribute(6, "width", 10);
                            builder.AddAttribute(7, "height", 10);
                            builder.CloseElement();
                        }
                    }
                }
            }
        }

        public string GetElementName(ShapeRenderType renderType)
            => renderType switch {
                ShapeRenderType.Polyline => "polyline",
                ShapeRenderType.Polygon => "polygon",
                _ => throw new NotImplementedException($"No implementation found for {nameof(ShapeRenderType)} '{renderType}'")
            };
    }
}
