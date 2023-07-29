using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class BarLayer : LayerBase {
    public static decimal DefaultClearancePercentage { get; set; } = 10M;
    public static decimal DefaultGapPercentage { get; set; } = 5M;

    public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;
    public decimal GapPercentage { get; set; } = DefaultGapPercentage;

    public BarLayer(XYChart chart) : base(chart) { }

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        if (IsStacked) {
            return GetStackedDataSeriesShapes();
        }
        else {
            return GetUnstackedDataSeriesShapes();
        }
    }

    public IEnumerable<ShapeBase> GetStackedDataSeriesShapes() {
        var width = Chart.DataPointWidth / 100M * (100M - ClearancePercentage * 2);
        var xOffset = width / 2M;

        return GetDataSeriesPoints().Select(point => new BarDataShape(
            point.X - xOffset,
            point.Y,
            width,
            point.Height,
            point.Color,
            point.DataSeriesIndex,
            point.Index
        ));
    }

    public IEnumerable<ShapeBase> GetUnstackedDataSeriesShapes() {
        var totalWidth = Chart.DataPointWidth / 100M * (100M - ClearancePercentage * 2);
        var gapWidth = Chart.DataPointWidth / 100M * GapPercentage;
        var dataSeriesWidth = (totalWidth - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;

        return GetDataSeriesPoints().Select(point => new BarDataShape(
            point.X
                + (point.DataSeriesIndex - DataSeries.Count / 2M) * dataSeriesWidth
                + (point.DataSeriesIndex - (DataSeries.Count - 1) / 2M) * gapWidth,
            point.Y,
            dataSeriesWidth,
            point.Height,
            point.Color,
            point.DataSeriesIndex,
            point.Index
        ));
    }
}
