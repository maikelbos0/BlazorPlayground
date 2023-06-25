namespace BlazorPlayground.Chart;

public static class DoubleExtensions {
    public static double FloorToScale(this double value, double scale)
        => Math.Floor(value / scale) * scale;

    public static double CeilingToScale(this double value, double scale)
        => Math.Ceiling(value / scale) * scale;

    private readonly static double[] scaleMultipliers = new[] { 2, 2.5, 2, 2 };

    public static double GetScale(double min, double max) {
        if (min == max) {
            return 1;
        }

        var delta = max - min;
        var scale = Math.Pow(10, Math.Floor(Math.Log10(delta) - 1));
        var i = 0;

        while ((max.CeilingToScale(scale) - min.FloorToScale(scale)) / scale > 8) {
            scale *= scaleMultipliers[i++];
        }

        return scale;
    }
}