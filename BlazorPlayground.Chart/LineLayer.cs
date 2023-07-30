using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class LineLayer : LayerBase {
    public static DataMarkerDelegate DefaultDataMarker { get; set; } = DefaultDataMarkers.Round;

    public DataMarkerDelegate DataMarker { get; set; } = DefaultDataMarker;

    // TODO setting for show marker
    // TODO setting for marker size
    // TODO setting for show line
    // TODO setting for what do do for null in line

    public LineLayer(XYChart chart) : base(chart) { }

    // TODO add actual lines
    public override IEnumerable<ShapeBase> GetDataSeriesShapes()
        => GetDataSeriesPoints().Select(point => DataMarker(
            point.X,
            point.Y,
            10M,
            point.Color,
            point.DataSeriesIndex,
            point.Index
        ));
}
