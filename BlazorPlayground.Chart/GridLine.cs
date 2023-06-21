namespace BlazorPlayground.Chart;

public class GridLine : Shape {
    public override string CssClass => "grid-line";
    public override string ElementName => "line";
    public int X { get; }
    public int Y { get; }
    public int Width { get; }

    public GridLine(int x, int y, int width) {
        X = x;
        Y = y;
        Width = width;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x1", X },
        { "y1", Y },
        { "x2", X + Width },
        { "y2", Y }
    };           
}