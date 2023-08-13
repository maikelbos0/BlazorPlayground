using BlazorPlayground.Chart.Shapes;
using Microsoft.AspNetCore.Components;

namespace BlazorPlayground.Chart;

public class AreaLayer : LayerBase {
    public static LineGapMode DefaultLineGapMode { get; set; } = LineGapMode.Skip;

    public override StackMode StackMode => StackMode.Single;
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.EdgeToEdge;

    [Parameter] public LineGapMode LineGapMode { get; set; } = DefaultLineGapMode;

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var dataPointsByDataSeries = GetCanvasDataPoints().ToLookup(dataSeriesPoint => dataSeriesPoint.DataSeriesIndex);
        var zeroY = Chart.MapDataPointToCanvas(0M);

        for (var dataSeriesIndex = 0; dataSeriesIndex < DataSeries.Count; dataSeriesIndex++) {
            var dataPoints = dataPointsByDataSeries[dataSeriesIndex].OrderBy(dataPoint => dataPoint.Index).ToList();

            if (dataPoints.Any()) {
                var commands = new List<string>();

                for (var i = 0; i < dataPoints.Count; i++) {
                    if (i == 0) {
                        commands.Add(PathCommandFactory.MoveTo(dataPoints[i].X, zeroY));
                        commands.Add(PathCommandFactory.LineTo(dataPoints[i].X, dataPoints[i].Y));
                    }
                    else if (dataPoints[i - 1].Index < dataPoints[i].Index - 1 && LineGapMode == LineGapMode.Skip) {
                        commands.Add(PathCommandFactory.LineTo(dataPoints[i - 1].X, zeroY));
                        commands.Add(PathCommandFactory.ClosePath);

                        commands.Add(PathCommandFactory.MoveTo(dataPoints[i].X, zeroY));
                        commands.Add(PathCommandFactory.LineTo(dataPoints[i].X, dataPoints[i].Y));
                    }
                    else {
                        commands.Add(PathCommandFactory.LineTo(dataPoints[i].X, dataPoints[i].Y));
                    }
                }

                commands.Add(PathCommandFactory.LineTo(dataPoints[^1].X, zeroY));
                commands.Add(PathCommandFactory.ClosePath);

                yield return new DataAreaShape(
                    commands,
                    DataSeries[dataSeriesIndex].GetColor(),
                    dataSeriesIndex
                );
            }
        }
    }
}
