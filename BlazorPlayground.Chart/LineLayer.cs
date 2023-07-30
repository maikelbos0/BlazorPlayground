using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class LineLayer : LayerBase {
    public static bool DefaultShowDataMarkers { get; set; } = true;
    public static DataMarkerDelegate DefaultDataMarkerType { get; set; } = DefaultDataMarkerTypes.Round;

    public DataMarkerDelegate DataMarkerType { get; set; } = DefaultDataMarkerType;
    public bool ShowDataMarkers { get; set; } = DefaultShowDataMarkers;

    // TODO setting for marker size
    // TODO setting for show line
    // TODO setting for what do do for null in line

    public LineLayer(XYChart chart) : base(chart) { }

    // TODO add actual lines
    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var dataSeriesPoints = GetDataSeriesPoints();

        if (ShowDataMarkers) {
            foreach (var dataSeriesPoint in dataSeriesPoints) {
                yield return DataMarkerType(
                    dataSeriesPoint.X,
                    dataSeriesPoint.Y,
                    10M,
                    dataSeriesPoint.Color,
                    dataSeriesPoint.DataSeriesIndex,
                    dataSeriesPoint.Index
                );
            }
        }
    }
}
