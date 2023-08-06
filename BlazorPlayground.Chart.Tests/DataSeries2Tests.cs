using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DataSeries2Tests {
    [Fact]
    public void GetColor() {
        var subject = new DataSeries2() {
            Color = "magenta"
        };

        Assert.Equal("magenta", subject.GetColor());
    }

    [Theory]
    [InlineData(0, "red")]
    [InlineData(1, "blue")]
    [InlineData(2, "green")]
    [InlineData(3, "red")]
    public void GetColor_Default(int index, string expectedColor) {
        DataSeries2.DefaultColors = new List<string>() { "red", "blue", "green" };

        var layer = new TestLayer();
        layer.DataSeries.Add(new DataSeries2() { Layer = layer });
        layer.DataSeries.Add(new DataSeries2() { Layer = layer });
        layer.DataSeries.Add(new DataSeries2() { Layer = layer });
        layer.DataSeries.Add(new DataSeries2() { Layer = layer });

        var subject = layer.DataSeries[index];

        Assert.Equal(expectedColor, subject.GetColor());
    }

    [Fact]
    public void GetColor_Fallback() {
        DataSeries2.DefaultColors = new();

        var layer = new TestLayer();
        var subject = new DataSeries2() { Layer = layer };

        layer.DataSeries.Add(subject);

        Assert.Equal(DataSeries2.FallbackColor, subject.GetColor());
    }
}
