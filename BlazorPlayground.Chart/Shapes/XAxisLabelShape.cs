namespace BlazorPlayground.Chart.Shapes;

public class XAxisLabelShape : ShapeBase {

    public override string CssClass => "x-axis-label";
    public override string ElementName => "text";
    public decimal X { get; }
    public decimal Y { get; }
    public string Label { get; }

    public XAxisLabelShape(decimal x, decimal y, string label, int index) : base(index) {
        X = x;
        Y = y;
        Label = label;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    public override string? GetContent() => Label;
}