namespace BlazorPlayground.Chart;

public class Canvas {
    public int Width { get; set; } = 1200;
    public int Height { get; set; } = 600;
    public int Padding { get; set; } = 10;
    public int XAxisLabelHeight { get; set; } = 100;
    public int XAxisLabelClearance { get; set; } = 10;
    public int YAxisLabelWidth { get; set; } = 100;
    public int YAxisLabelClearance { get; set; } = 10;

    public int PlotAreaX => Padding + YAxisLabelWidth;
    public int PlotAreaY => Padding;
    public int PlotAreaWidth => Width - Padding * 2 - YAxisLabelWidth;
    public int PlotAreaHeight => Height - Padding * 2 - XAxisLabelHeight;
}
