using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class BarDataSeriesLayer : DataSeriesLayer {
    // TODO fix x axis to include configurable width and offset
    public static decimal DefaultClearancePercentage { get; set; } = 10M;
    
    public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;

    public BarDataSeriesLayer(XYChart chart) : base(chart) { }

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var zeroY = Chart.MapDataPointToCanvas(0M);
        var totalWidth = Chart.DataPointWidth / 100M * (100M - ClearancePercentage * 2);
        var dataSeriesWidth = totalWidth / DataSeries.Count;

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
                    Chart.MapDataIndexToCanvas(value.Index) + (dataSeriesIndex - DataSeries.Count / 2M) * dataSeriesWidth, // TODO gap
                    y,
                    dataSeriesWidth, // TODO gap
                    height,
                    dataSeries.Color,
                    dataSeriesIndex,
                    value.Index
                );
            }));
    }
}
