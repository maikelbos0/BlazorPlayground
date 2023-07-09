namespace BlazorPlayground.Chart.Shapes;

public class YAxisMultiplierShape : ShapeBase {
    public override string CssClass => "y-axis-multiplier";
    public override string ElementName => "text";
    public decimal X { get; }
    public decimal Y { get; }
    public string Multiplier { get; }

    public YAxisMultiplierShape(decimal x, decimal y, string multiplier) {
        X = x;
        Y = y;
        Multiplier = multiplier;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    public override string? GetContent() => Multiplier;
}
