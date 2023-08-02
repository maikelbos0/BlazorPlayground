namespace BlazorPlayground.Chart.Shapes;

public static class PathCommandFactory {
    public static string MoveTo(decimal x, decimal y) => FormattableString.Invariant($"M {x} {y}");

    public static string LineTo(decimal x, decimal y) => FormattableString.Invariant($"L {x} {y}");
}
