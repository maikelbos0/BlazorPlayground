﻿namespace BlazorPlayground.Chart;

public class Canvas {
    public static int DefaultWidth { get; set; } = 1200;
    public static int DefaultHeight { get; set; } = 600;
    public static int DefaultPadding { get; set; } = 25;
    public static int DefaultXAxisLabelHeight { get; set; } = 100;
    public static int DefaultXAxisLabelClearance { get; set; } = 10;
    public static int DefaultYAxisLabelWidth { get; set; } = 100;
    public static int DefaultYAxisLabelClearance { get; set; } = 10;

    public int Width { get; set; } = DefaultWidth;
    public int Height { get; set; } = DefaultHeight;
    public int Padding { get; set; } = DefaultPadding;
    public int XAxisLabelHeight { get; set; } = DefaultXAxisLabelHeight;
    public int XAxisLabelClearance { get; set; } = DefaultXAxisLabelClearance;
    public int YAxisLabelWidth { get; set; } = DefaultYAxisLabelWidth;
    public int YAxisLabelClearance { get; set; } = DefaultYAxisLabelClearance;
    public int PlotAreaX => Padding + YAxisLabelWidth;
    public int PlotAreaY => Padding;
    public int PlotAreaWidth => Width - Padding * 2 - YAxisLabelWidth;
    public int PlotAreaHeight => Height - Padding * 2 - XAxisLabelHeight;

    public Shapes.PlotAreaShape GetPlotAreaShape() => new(PlotAreaX, PlotAreaY, PlotAreaWidth, PlotAreaHeight);
}
