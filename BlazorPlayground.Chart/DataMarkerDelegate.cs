using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public delegate ShapeBase DataMarkerDelegate(decimal x, decimal y, decimal size, string color, int dataSeriesIndex, int dataPointIndex);
