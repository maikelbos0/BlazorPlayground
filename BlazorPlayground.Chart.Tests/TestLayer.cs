using BlazorPlayground.Chart.Shapes;
using System.Collections.Generic;

namespace BlazorPlayground.Chart.Tests;

public class TestLayer : LayerBase2 {
    public override StackMode StackMode => throw new System.NotImplementedException();

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() => throw new System.NotImplementedException();
}
