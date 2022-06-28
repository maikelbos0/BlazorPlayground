using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Graphics {
    public class Shape : ComponentBase {
        [Parameter]
        public IPointSeries? PointSeries { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (PointSeries != null) {
                builder.OpenElement(1, GetElementName(PointSeries.GetSeriesType()));
                builder.AddAttribute(2, "style", "stroke: #000; stroke-width: 2; fill: transparent;");
                builder.AddAttribute(3, "points", string.Join(" ", PointSeries.GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))));
                builder.CloseElement();
            }
        }

        public string GetElementName(PointSeriesType seriesType)
            => seriesType switch {
                PointSeriesType.Polyline => "polyline",
                PointSeriesType.Polygon => "polygon",
                _ => throw new NotImplementedException($"No implementation found for {nameof(PointSeriesType)} '{seriesType}'")
            };
}
}
