namespace BlazorPlayground.Chart.Shapes;

public class SquareDataMarkerShape : ShapeBase {
    public override string CssClass => "data-marker data-marker-square";
    public override string ElementName => "rect";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Size { get; }
    public string Color { get; }

    public SquareDataMarkerShape(decimal x, decimal y, decimal size, string color, int dataSeriesIndex, int dataPointIndex) : base(dataSeriesIndex, dataPointIndex) {
        X = x;
        Y = y;
        Size = size;
        Color = color;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X - Size / 2M },
        { "y", Y - Size / 2M },
        { "width", Size },
        { "height", Size },
        { "fill", Color }
    };
}
