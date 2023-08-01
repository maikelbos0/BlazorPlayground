using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class LineLayer : LayerBase {
    public static bool DefaultShowDataMarkers { get; set; } = true;
    public static decimal DefaultDataMarkerSize { get; set; } = 10M;
    public static DataMarkerDelegate DefaultDataMarkerType { get; set; } = DefaultDataMarkerTypes.Round;

    public bool ShowDataMarkers { get; set; } = DefaultShowDataMarkers;
    public decimal DataMarkerSize { get; set; } = DefaultDataMarkerSize;
    public DataMarkerDelegate DataMarkerType { get; set; } = DefaultDataMarkerType;
    public override StackMode StackMode => StackMode.Single;

    // TODO setting for show line
    // TODO setting for line width
    // TODO setting for what do do for null in line
    // TODO fluent lines?

    public LineLayer(XYChart chart) : base(chart) { }

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var dataPoints = GetDataPoints();

        if (ShowDataMarkers) {
            foreach (var dataPoint in dataPoints) {
                yield return DataMarkerType(
                    dataPoint.X,
                    dataPoint.Y,
                    DataMarkerSize,
                    dataPoint.Color,
                    dataPoint.DataSeriesIndex,
                    dataPoint.Index
                );
            }
        }

        var dataSeries = dataPoints.ToLookup(dataSeriesPoint => dataSeriesPoint.DataSeriesIndex);

        for (var dataSeriesIndex = 0; dataSeriesIndex < DataSeries.Count; dataSeriesIndex++) {
            var dataSeriesPoints = dataSeries[dataSeriesIndex].OrderBy(dataPoint => dataPoint.Index).ToList();

            for (var index = 0; index < dataSeriesPoints.Count - 1; index++) {
                var startPoint = dataSeriesPoints[index];
                var endPoint = dataSeriesPoints[index + 1];

                yield return new LineDataShape(
                    startPoint.X,
                    startPoint.Y,
                    endPoint.X,
                    endPoint.Y,
                    startPoint.Color,
                    dataSeriesIndex,
                    startPoint.Index
                );
            }
        }
    }
}
