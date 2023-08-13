using BlazorPlayground.Chart.Shapes;
using System;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DataSeriesTests {
    private class TestLayer : LayerBase {
        public override StackMode StackMode => throw new NotImplementedException();
        public override DataPointSpacingMode DefaultDataPointSpacingMode => throw new NotImplementedException();

        public override IEnumerable<ShapeBase> GetDataSeriesShapes() => throw new NotImplementedException();
    }

    [Fact]
    public void GetColor() {
        var subject = new DataSeries() {
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
        DataSeries.DefaultColors = new List<string>() { "red", "blue", "green" };

        var layer = new TestLayer();
        layer.DataSeries.Add(new DataSeries() { Layer = layer });
        layer.DataSeries.Add(new DataSeries() { Layer = layer });
        layer.DataSeries.Add(new DataSeries() { Layer = layer });
        layer.DataSeries.Add(new DataSeries() { Layer = layer });

        var subject = layer.DataSeries[index];

        Assert.Equal(expectedColor, subject.GetColor());
    }

    [Fact]
    public void GetColor_Fallback() {
        DataSeries.DefaultColors = new();

        var layer = new TestLayer();
        var subject = new DataSeries() { Layer = layer };

        layer.DataSeries.Add(subject);

        Assert.Equal(DataSeries.FallbackColor, subject.GetColor());
    }
}
