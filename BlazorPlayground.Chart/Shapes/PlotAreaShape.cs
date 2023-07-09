namespace BlazorPlayground.Chart.Shapes;

public class PlotAreaShape : ShapeBase {
    private const int bleed = 100;

    public override string CssClass => "plot-area";
    public override string ElementName => "path";
    public int CanvasWidth { get; }
    public int CanvasHeight { get; }
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    public PlotAreaShape(int canvasWidth, int canvasHeight, int x, int y, int width, int height) {
        CanvasWidth = canvasWidth;
        CanvasHeight = canvasHeight;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", $"M{-bleed} {-bleed} l{CanvasWidth + bleed * 2} 0 l0 {CanvasHeight + bleed * 2} l{-CanvasWidth - bleed * 2} 0 Z M{X} {Y} l{Width} 0 l0 {Height} l{-Width} 0 Z" },
        { "fill-rule", "evenodd" }
    };
}