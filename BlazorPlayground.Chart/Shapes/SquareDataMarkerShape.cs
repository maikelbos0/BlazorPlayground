namespace BlazorPlayground.Chart.Shapes;

public class SquareDataMarkerShape : ShapeBase {
    public override string CssClass => "data-marker data-marker-square";
    public override string ElementName => "rect";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Width { get; }
    public decimal Height { get; }
    public string Color { get; }

    public SquareDataMarkerShape(decimal x, decimal y, decimal width, decimal height, string color, int dataSeriesIndex, int dataPointIndex) : base(dataSeriesIndex, dataPointIndex) {
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
