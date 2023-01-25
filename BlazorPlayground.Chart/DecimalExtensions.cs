namespace BlazorPlayground.Chart;

public static class DecimalExtensions {
    public static decimal FloorToScale(this decimal value, decimal scale)
        => Math.Floor(value / scale) * scale;

    public static decimal CeilingToScale(this decimal value, decimal scale)
        => Math.Ceiling(value / scale) * scale;

    private static decimal[] scaleMultipliers = new[] { 2M, 2.5M, 2M };

    public static decimal GetScale(decimal min, decimal max) {
        var delta = max - min;
        var scale = (decimal)Math.Pow(10, Math.Floor(Math.Log10((double)delta) - 1));
        var i = 0;

        // TODO consider adjusting scaling bars?
        while ((max.CeilingToScale(scale) - min.FloorToScale(scale)) / scale > 5) {
            scale *= scaleMultipliers[i++];

            i %= scaleMultipliers.Length;
        }

        return scale;
    }
}