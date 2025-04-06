﻿using BlazorPlayground.Graphics.Geometries;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests.Geometries;

public class DrawableShapeGeometryFactoryTests {
    [Fact]
    public void GetGeometryFromEmptyEnumerable() {
        var geometryFactory = new DrawableShapeGeometry();

        var result = geometryFactory.GetGeometry(Enumerable.Empty<DrawableShape>());

        Assert.Null(result);
    }
}
