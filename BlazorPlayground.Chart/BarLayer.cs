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
        var negativeOffsets = Enumerable.Repeat(0M, Chart.Labels.Count).ToList();
        var positiveOffsets = Enumerable.Repeat(0M, Chart.Labels.Count).ToList();

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => {
                var dataPoint = value.DataPoint!.Value;
                var offsets = dataPoint < 0 ? negativeOffsets : positiveOffsets;

                offsets[value.Index] += dataPoint;

                return new BarDataShape(
                    Chart.MapDataIndexToCanvas(value.Index) - xOffset,
                    Chart.MapDataPointToCanvas(offsets[value.Index]),
                    width,
                    Chart.MapDataValueToPlotArea(dataPoint),
                    dataSeries.Color,
                    dataSeriesIndex,
                    value.Index
                );
            }));
    }

    public IEnumerable<ShapeBase> GetUnstackedDataSeriesShapes() {
        var totalWidth = Chart.DataPointWidth / 100M * (100M - ClearancePercentage * 2);
        var gapWidth = Chart.DataPointWidth / 100M * GapPercentage;
        var dataSeriesWidth = (totalWidth - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => {
                return new BarDataShape(
                    Chart.MapDataIndexToCanvas(value.Index)
                        + (dataSeriesIndex - DataSeries.Count / 2M) * dataSeriesWidth
                        + (dataSeriesIndex - (DataSeries.Count - 1) / 2M) * gapWidth,
                    Chart.MapDataPointToCanvas(value.DataPoint!.Value),
                    dataSeriesWidth,
                    Chart.MapDataValueToPlotArea(value.DataPoint!.Value),
                    dataSeries.Color,
                    dataSeriesIndex,
                    value.Index
                );
            }));
    }
}
