using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class LineLayer : LayerBase {
    public static bool DefaultShowDataMarkers { get; set; } = true;
    public static decimal DefaultDataMarkerSize { get; set; } = 10M;
    public static DataMarkerDelegate DefaultDataMarkerType { get; set; } = DefaultDataMarkerTypes.Round;
    public static bool DefaultShowDataLines { get; set; } = true;
    public static decimal DefaultDataLineWidth { get; set; } = 2M;

    public bool ShowDataMarkers { get; set; } = DefaultShowDataMarkers;
    public decimal DataMarkerSize { get; set; } = DefaultDataMarkerSize;
    public DataMarkerDelegate DataMarkerType { get; set; } = DefaultDataMarkerType;
    public bool ShowDataLines { get; set; } = DefaultShowDataLines;
    public decimal DataLineWidth { get; set; } = DefaultDataLineWidth;
    public override StackMode StackMode => StackMode.Single;

    // TODO setting for what do do for null in line
    // TODO fluent lines?

    public LineLayer(XYChart chart) : base(chart) { }

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var dataPointsByDataSeries = GetDataPoints().ToLookup(dataSeriesPoint => dataSeriesPoint.DataSeriesIndex);

        for (var dataSeriesIndex = 0; dataSeriesIndex < DataSeries.Count; dataSeriesIndex++) {
            var dataPoints = dataPointsByDataSeries[dataSeriesIndex].OrderBy(dataPoint => dataPoint.Index).ToList();

            if (ShowDataMarkers) {
                foreach (var dataPoint in dataPoints) {
                    yield return DataMarkerType(
                        dataPoint.X,
                        dataPoint.Y,
                        DataMarkerSize,
                        DataSeries[dataSeriesIndex].Color,
                        dataSeriesIndex,
                        dataPoint.Index
                    );
                }
            }

            if (ShowDataLines && dataPoints.Any()) {
                var commands = new List<string>() {
                    PathCommandFactory.MoveTo(dataPoints[0].X, dataPoints[0].Y)
                };

                foreach (var dataPoint in dataPoints.Skip(1)) {
                    commands.Add(PathCommandFactory.LineTo(dataPoint.X, dataPoint.Y));
                }

                yield return new DataLineShape(
                    commands,
                    DataLineWidth,
                    DataSeries[dataSeriesIndex].Color,
                    dataSeriesIndex
                );
            }
        }
    }
}
