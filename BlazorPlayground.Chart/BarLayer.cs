using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class BarLayer : LayerBase {
    public static decimal DefaultClearancePercentage { get; set; } = 10M;
    public static decimal DefaultGapPercentage { get; set; } = 5M;

    public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;
    public decimal GapPercentage { get; set; } = DefaultGapPercentage;
    public override StackMode StackMode => StackMode.Split;

    public BarLayer(XYChart chart) : base(chart) { }

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var width = Chart.DataPointWidth / 100M * (100M - ClearancePercentage * 2);
        Func<int, decimal> offsetProvider = dataSeriesIndex => -width / 2M;

        if (!IsStacked) {
            var gapWidth = Chart.DataPointWidth / 100M * GapPercentage;
            var dataSeriesWidth = (width - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;

            width = (width - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;
            offsetProvider = dataSeriesIndex => (dataSeriesIndex - DataSeries.Count / 2M) * dataSeriesWidth + (dataSeriesIndex - (DataSeries.Count - 1) / 2M) * gapWidth;
        }

        return GetDataPoints().Select(point => new BarDataShape(
            point.X + offsetProvider(point.DataSeriesIndex),
            point.Y,
            width,
            point.Height,
            point.Color,
            point.DataSeriesIndex,
            point.Index
        ));
    }
}
