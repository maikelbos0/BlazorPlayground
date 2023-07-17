using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class BarDataSeriesLayer : DataSeriesLayer {
    // TODO fix x axis to include configurable width and offset

    public BarDataSeriesLayer(XYChart chart) : base(chart) { }

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var zeroY = Chart.MapDataPointToCanvas(0M);

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
                    Chart.MapDataIndexToCanvas(value.Index) - 5, // TODO width, position and gap
                    y,
                    10, // TODO width, position and gap
                    height,
                    dataSeries.Color,
                    dataSeriesIndex,
                    value.Index
                );
            }));
    }
}
