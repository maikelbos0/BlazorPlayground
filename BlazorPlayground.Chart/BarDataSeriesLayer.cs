using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class BarDataSeriesLayer : DataSeriesLayer {
    public static decimal DefaultClearancePercentage { get; set; } = 10M;
    public static decimal DefaultGapPercentage { get; set; } = 5M;

    public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;
    public decimal GapPercentage { get; set; } = DefaultGapPercentage;

    public BarDataSeriesLayer(XYChart chart) : base(chart) { }

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
        var offset = width / 2M;
        var minimums = Enumerable.Repeat(0M, Chart.Labels.Count).ToList();
        var maximums = Enumerable.Repeat(0M, Chart.Labels.Count).ToList();

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => {
                var dataPoint = value.DataPoint!.Value;
                var dataHeight = Math.Abs(dataPoint);
                decimal y;

                if (dataPoint < 0) {
                    y = Chart.MapDataPointToCanvas(minimums[value.Index]);
                    minimums[value.Index] -= dataHeight;
                }
                else {
                    maximums[value.Index] += dataHeight;
                    y = Chart.MapDataPointToCanvas(maximums[value.Index]);
                }

                return new BarDataShape(
                    Chart.MapDataIndexToCanvas(value.Index) - offset,
                    y,
                    width,
                    Chart.MapDataValueToPlotArea(dataHeight),
                    dataSeries.Color,
                    dataSeriesIndex,
                    value.Index
                );
            }));
    }

    public IEnumerable<ShapeBase> GetUnstackedDataSeriesShapes() {
        var zeroY = Chart.MapDataPointToCanvas(0M);
        var totalWidth = Chart.DataPointWidth / 100M * (100M - ClearancePercentage * 2);
        var gapWidth = Chart.DataPointWidth / 100M * GapPercentage;
        var dataSeriesWidth = (totalWidth - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => {
                var y = Chart.MapDataPointToCanvas(value.DataPoint!.Value);
                decimal height;

                if (y < zeroY) {
                    height = zeroY - y;
                }
                else {
                    height = y - zeroY;
                    y = zeroY;
                }

                return new BarDataShape(
                    Chart.MapDataIndexToCanvas(value.Index)
                        + (dataSeriesIndex - DataSeries.Count / 2M) * dataSeriesWidth
                        + (dataSeriesIndex - (DataSeries.Count - 1) / 2M) * gapWidth,
                    y,
                    dataSeriesWidth,
                    height,
                    dataSeries.Color,
                    dataSeriesIndex,
                    value.Index
                );
            }));
    }
}
