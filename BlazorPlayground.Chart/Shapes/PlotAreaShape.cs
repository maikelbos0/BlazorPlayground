namespace BlazorPlayground.Chart.Shapes;

public class PlotAreaShape : ShapeBase {
    public override string CssClass => "plot-area";
    public override string ElementName => "rect";
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    public PlotAreaShape(int x, int y, int width, int height) {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y },
        { "width", Width },
        { "height", Height }
    };
}