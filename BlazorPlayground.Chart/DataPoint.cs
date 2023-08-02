namespace BlazorPlayground.Chart;

// TODO remove color?
public record class DataPoint(decimal X, decimal Y, decimal Height, string Color, int DataSeriesIndex, int Index);
