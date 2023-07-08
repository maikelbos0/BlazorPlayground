using System.Globalization;

namespace BlazorPlayground.Chart.Shapes;

public class YAxisLabelShape : ShapeBase {
    public override string CssClass => "y-axis-label";
    public override string ElementName => "text";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Value { get; }

    public YAxisLabelShape(decimal x, decimal y, decimal value) {
        X = x;
        Y = y;
        Value = value;
    }

    public override string GetKey() => $"{nameof(YAxisLabelShape)}/{X}/{Y}/{Value}";

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    // TODO apply scaling
    public override string? GetContent() => Value.ToString("N2", CultureInfo.InvariantCulture);
}
