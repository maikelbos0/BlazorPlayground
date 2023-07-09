namespace BlazorPlayground.Chart.Shapes;

public class GridLineShape : ShapeBase {
    public override string CssClass => "grid-line";
    public override string ElementName => "line";
    public decimal X { get; }
    public decimal Y { get; }
    public int Width { get; }
    public decimal Value { get; }

    public GridLineShape(decimal x, decimal y, int width, decimal value, int index) : base(index) {
        X = x;
        Y = y;
        Width = width;
        Value = value;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x1", X },
        { "y1", Y },
        { "x2", X + Width },
        { "y2", Y },
        { "value", Value }
    };
}