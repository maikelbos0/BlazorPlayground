using System.Globalization;

namespace BlazorPlayground.Chart.Shapes;

public class YAxisLabelShape : ShapeBase {
    public override string CssClass => "y-axis-label";
    public override string ElementName => "text";
    public double X { get; }
    public double Y { get; }
    public double Value { get; }

    public YAxisLabelShape(double x, double y, double value) {
        X = x;
        Y = y;
        Value = value;
    }
    
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    // TODO apply scaling
    public override string? GetContent() => Value.ToString("N2", CultureInfo.InvariantCulture);
}
