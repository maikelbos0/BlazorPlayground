using BlazorPlayground.Chart.Shapes;
using Microsoft.AspNetCore.Components;

namespace BlazorPlayground.Chart;

public class BarLayer : LayerBase {
    public static decimal DefaultClearancePercentage { get; set; } = 10M;
    public static decimal DefaultGapPercentage { get; set; } = 5M;

    [Parameter] public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;
    [Parameter] public decimal GapPercentage { get; set; } = DefaultGapPercentage;
    public override StackMode StackMode => StackMode.Split;
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.Center;

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var width = Chart.GetDataPointWidth() / 100M * (100M - ClearancePercentage * 2);
        Func<int, decimal> offsetProvider = dataSeriesIndex => -width / 2M;

        if (!IsStacked) {
            var gapWidth = Chart.GetDataPointWidth() / 100M * GapPercentage;
            var dataSeriesWidth = (width - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;

            width = (width - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;
            offsetProvider = dataSeriesIndex => (dataSeriesIndex - DataSeries.Count / 2M) * dataSeriesWidth + (dataSeriesIndex - (DataSeries.Count - 1) / 2M) * gapWidth;
        }

        return GetCanvasDataPoints().Select(point => new DataBarShape(
            point.X + offsetProvider(point.DataSeriesIndex),
            point.Y,
            width,
            point.Height,
            DataSeries[point.DataSeriesIndex].GetColor(),
            point.DataSeriesIndex,
            point.Index
        ));
    }
}
