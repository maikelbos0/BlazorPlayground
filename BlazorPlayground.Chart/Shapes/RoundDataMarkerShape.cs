namespace BlazorPlayground.Chart.Shapes;

public class RoundDataMarkerShape : ShapeBase {
    public override string CssClass => "data-marker data-marker-round";
    public override string ElementName => "circle";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Radius { get; }
    public string Color { get; }

    public RoundDataMarkerShape(decimal x, decimal y, decimal radius, string color, int dataSeriesIndex, int dataPointIndex) : base(dataSeriesIndex, dataPointIndex) {
        X = x;
        Y = y;
        Radius = radius;
        Color = color;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "cx", X },
        { "cy", Y },
        { "r", Radius },
        { "fill", Color }   
    };
}
