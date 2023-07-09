namespace BlazorPlayground.Chart.Shapes;

public class YAxisLabelShape : ShapeBase {
    public override string CssClass => "y-axis-label";
    public override string ElementName => "text";
    public decimal X { get; }
    public decimal Y { get; }
    public string Value { get; }

    public YAxisLabelShape(decimal x, decimal y, string value, int index) : base(index) {
        X = x;
        Y = y;
        Value = value;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    public override string? GetContent() => Value;
}
