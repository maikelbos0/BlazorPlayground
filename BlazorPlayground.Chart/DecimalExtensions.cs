namespace BlazorPlayground.Chart; 

public static class Grid {
    public static decimal FloorToScale(this decimal value, decimal scale)
        => Math.Floor(value / scale) * scale;

    public static decimal CeilingToScale(this decimal value, decimal scale)
        => Math.Ceiling(value / scale) * scale;

}