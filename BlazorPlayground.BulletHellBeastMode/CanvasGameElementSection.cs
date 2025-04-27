namespace BlazorPlayground.BulletHellBeastMode;

public record CanvasGameElementSection(string Type, List<(double X, double Y)> Coordinates, string FillColor, string StrokeColor, int StrokeWidth, double Opacity);
