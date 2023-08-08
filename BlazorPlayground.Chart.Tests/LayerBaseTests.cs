using BlazorPlayground.Chart.Shapes;
using System.Collections.Generic;
using System;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class LayerBaseTests {
    private class TestLayer : LayerBase {
        public override StackMode StackMode { get; }

        public TestLayer(StackMode stackMode) {
            StackMode = stackMode;
        }

        public override IEnumerable<ShapeBase> GetDataSeriesShapes() => throw new NotImplementedException();
    }


    [Theory]
    [MemberData(nameof(GetScaleDataPoints_Data))]
    public void GetScaleDataPoints(bool isStacked, StackMode stackMode, decimal[] expectedDataPoints) {
        var subject = new TestLayer(stackMode) {
            Chart = new() {
                Labels = { "Foo", "Bar", "Baz", "Quux" }
            },
            IsStacked = isStacked,
            DataSeries = {
                new() {
                    DataPoints = { -5M, -3M, null, null, 15M }
                },
                new() {
                    DataPoints = { -7M, -3M, null, null, 15M }
                },
                new() {
                    DataPoints = { 7M, null, 3M }
                },
                new() {
                    DataPoints = { 5M, null, 3M }
                }
            }
        };

        Assert.Equal(expectedDataPoints, subject.GetScaleDataPoints());
    }

    public static TheoryData<bool, StackMode, decimal[]> GetScaleDataPoints_Data() => new() {
        { false, StackMode.Single, new[] { -5M, -3M, -7M, -3M, 7M, 3M, 5M, 3M } },
        { false, StackMode.Split, new[] { -5M, -3M, -7M, -3M, 7M, 3M, 5M, 3M } },
        { true, StackMode.Single, new[] { -5M, -3M, -12M, -6M, -5M, 3M, 0M, 6M } },
        { true, StackMode.Split, new[] { -5M, -3M, -12M, -6M, 7M, 3M, 12M, 6M } }
    };
}
