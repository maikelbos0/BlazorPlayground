namespace BlazorPlayground.Chart;

public class XYChart {
    private const int autoScaleDefaultMin = 0;
    private const int autoScaleDefaultMax = 5;
    private const int autoScaleMaxGridLineCount = 8;
    private static double[] autoScaleMultipliers = new[] { 2, 2.5, 2, 2 };

    private double? gridLineInterval;
    private double? plotAreaMin;
    private double? plotAreaMax;

    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();
    public double GridLineInterval {
        get {
            if (gridLineInterval == null) {
                ScalePlotArea();
            }

            return gridLineInterval.Value;
        }
        set => gridLineInterval = value;
    }
    public double PlotAreaMin {
        get {
            if (plotAreaMin == null) {
                ScalePlotArea();
            }

            return plotAreaMin.Value;
        }
        set => plotAreaMin = value;
    }
    public double PlotAreaMax {
        get {
            if (plotAreaMax == null) {
                ScalePlotArea();
            }

            return plotAreaMax.Value;
        }
        set => plotAreaMax = value;
    }

    private void ScalePlotArea() {
        var dataPoints = DataSeries.SelectMany(series => series).Where(dataPoint => dataPoint != null).Cast<double>();
        var plotAreaMin = this.plotAreaMin ?? dataPoints.DefaultIfEmpty(autoScaleDefaultMin).Min();
        var plotAreaMax = this.plotAreaMax ?? dataPoints.DefaultIfEmpty(autoScaleDefaultMax).Max();
        var gridLineInterval = this.gridLineInterval;

        if (plotAreaMin == plotAreaMax) {
            plotAreaMin -= (autoScaleDefaultMax - autoScaleDefaultMin) / 2;
            plotAreaMax += (autoScaleDefaultMax - autoScaleDefaultMin) / 2;
        }

        gridLineInterval ??= GetGridLineInterval(plotAreaMin, plotAreaMax);

        this.gridLineInterval ??= gridLineInterval;
        this.plotAreaMin ??= plotAreaMin.FloorToScale(this.gridLineInterval.Value);
        this.plotAreaMax ??= plotAreaMax.CeilingToScale(this.gridLineInterval.Value);

        double GetGridLineInterval(double plotAreaMin, double plotAreaMax) {
            var interval = Math.Pow(10, Math.Floor(Math.Log10(plotAreaMin - plotAreaMax) - 1));
            var i = 0;

            while ((plotAreaMax.CeilingToScale(interval) - plotAreaMin.FloorToScale(interval)) / interval > autoScaleMaxGridLineCount) {
                interval *= autoScaleMultipliers[i++];
            }

            return interval;
        }
    }
}
