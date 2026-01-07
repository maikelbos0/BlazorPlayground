using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics;

public static class ShapeWithStrokeLinejoinExtensions {
    private class Data {
        public Linejoin StrokeLinejoin { get; set; }
    }

    private static readonly ConditionalWeakTable<IShapeWithStrokeLinejoin, Data> shapes = [];

    extension(IShapeWithStrokeLinejoin shape) {
        public Linejoin StrokeLinejoin {
            get => shapes.GetOrCreateValue(shape).StrokeLinejoin;
            set => shapes.GetOrCreateValue(shape).StrokeLinejoin = value;
        }
    }
}
