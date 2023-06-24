using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DataSeriesTests {
    [Theory]
    [InlineData(0, "red")]
    [InlineData(1, "blue")]
    [InlineData(2, "green")]
    [InlineData(3, "red")]
    public void GetColor(int index, string expectedColor) {
        DataSeries.DefaultColors = new List<string>() { "red", "blue", "green" };

        Assert.Equal(expectedColor, DataSeries.GetColor(index));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1, "red", "red", "blue", "green")]
    public void GetColor_Fallback(int index, params string[] defaultColors) {
        DataSeries.DefaultColors = defaultColors.ToList();

        Assert.Equal(DataSeries.FallbackColor, DataSeries.GetColor(index));
    }
}
