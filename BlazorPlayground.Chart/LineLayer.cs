using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class LineLayer : LayerBase {
    public static bool DefaultShowDataMarkers { get; set; } = true;
    public static decimal DefaultDataMarkerSize { get; set; } = 10M;
    public static DataMarkerDelegate DefaultDataMarkerType { get; set; } = DefaultDataMarkerTypes.Round;

    public bool ShowDataMarkers { get; set; } = DefaultShowDataMarkers;
    public decimal DataMarkerSize { get; set; } = DefaultDataMarkerSize;
    public DataMarkerDelegate DataMarkerType { get; set; } = DefaultDataMarkerType;

    // TODO setting for show line
    // TODO setting for what do do for null in line

    public LineLayer(XYChart chart) : base(chart) { }

    // TODO add actual lines
    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var dataSeriesPoints = GetDataPoints();

        if (ShowDataMarkers) {
            foreach (var dataSeriesPoint in dataSeriesPoints) {
                yield return DataMarkerType(
                    dataSeriesPoint.X,
                    dataSeriesPoint.Y,
                    DataMarkerSize,
                    dataSeriesPoint.Color,
                    dataSeriesPoint.DataSeriesIndex,
                    dataSeriesPoint.Index
                );
            }
        }
    }
}
