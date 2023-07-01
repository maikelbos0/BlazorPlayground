namespace BlazorPlayground.Chart.Shapes;

public class BarDataShape : ShapeBase {
    public override string CssClass => "bar-data";
    public override string ElementName => "rect";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Width { get; }
    public decimal Height { get; }
    public string Color { get; }

    public BarDataShape(decimal x, decimal y, decimal width, decimal height, string color) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Color = color;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y },
        { "width", Width },
        { "height", Height },
        { "fill", Color }
    };
}