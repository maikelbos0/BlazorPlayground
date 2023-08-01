namespace BlazorPlayground.Chart.Shapes;

public class DataLineShape : ShapeBase {
    public override string CssClass => "line-data";
    public override string ElementName => "line";
    public decimal X1 { get; }
    public decimal Y1 { get; }
    public decimal X2 { get; }
    public decimal Y2 { get; }
    public decimal Width { get; }
    public string Color { get; }

    public DataLineShape(decimal x1, decimal y1, decimal x2, decimal y2, decimal width, string color, int dataSeriesIndex, int dataPointIndex) : base(dataSeriesIndex, dataPointIndex) {
        Y1 = y1;
        X1 = x1;
        Y2 = y2;
        X2 = x2;
        Width = width;
        Color = color;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x1", X1 },
        { "y1", Y1 },
        { "x2", X2 },
        { "y2", Y2 },
        { "stroke-width", Width },
        { "stroke", Color }
    };
}
