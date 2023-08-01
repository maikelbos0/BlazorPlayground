namespace BlazorPlayground.Chart.Shapes;

public class DataBarShape : ShapeBase {
    public override string CssClass => "bar-data";
    public override string ElementName => "rect";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Width { get; }
    public decimal Height { get; }
    public string Color { get; }

    public DataBarShape(decimal x, decimal y, decimal width, decimal height, string color, int dataSeriesIndex, int dataPointIndex) : base(dataSeriesIndex, dataPointIndex) {
        Y = y;
        X = x;
        Height = height;
        Width = width;
        Color = color;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Height < 0 ? Y + Height : Y },
        { "width", Width },
        { "height", Math.Abs(Height) },
        { "fill", Color }
    };
}