using BlazorPlayground.Chart.Shapes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class DataSeriesLayerTests {
    public class TestDataSeriesLayer : DataSeriesLayer {
        public TestDataSeriesLayer(XYChart chart) : base(chart) { }

        public override IEnumerable<ShapeBase> GetDataSeriesShapes() => throw new System.NotImplementedException();
    }

    [Theory]
    [InlineData(0, "red")]
    [InlineData(1, "blue")]
    [InlineData(2, "green")]
    [InlineData(3, "red")]
    public void GetColor(int index, string expectedColor) {
        DataSeriesLayer.DefaultColors = new List<string>() { "red", "blue", "green" };

        Assert.Equal(expectedColor, DataSeriesLayer.GetColor(index));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1, "red", "red", "blue", "green")]
    public void GetColor_Fallback(int index, params string[] defaultColors) {
        DataSeriesLayer.DefaultColors = defaultColors.ToList();

        Assert.Equal(DataSeriesLayer.FallbackColor, DataSeriesLayer.GetColor(index));
    }

    [Fact]
    public void AddDataSeries_With_Color() {
        var subject = new TestDataSeriesLayer(
            new XYChart() {
                Labels = {
                    "Foo",
                    "Bar",
                    "Baz"
                }
            }
        );

        var result = subject.AddDataSeries("Data", "red");

        Assert.Same(result, Assert.Single(subject.DataSeries));
        Assert.Equal("Data", result.Name);
        Assert.Equal("red", result.Color);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void AddDataSeries_Without_Color() {
        DataSeriesLayer.DefaultColors = new List<string>() { "red", "blue", "green" };

        var subject = new TestDataSeriesLayer(
            new XYChart() {
                Labels = {
                    "Foo",
                    "Bar",
                    "Baz"
                }
            }
        );

        var result = subject.AddDataSeries("Data");

        Assert.Same(result, Assert.Single(subject.DataSeries));
        Assert.Equal("Data", result.Name);
        Assert.Equal("red", result.Color);
        Assert.Equal(3, result.Count);
    }
}
