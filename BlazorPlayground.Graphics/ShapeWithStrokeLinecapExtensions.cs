using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics;

public static class ShapeWithStrokeLinecapExtensions {
    private class Data {
        public Linecap StrokeLinecap { get; set; }
    }

    private static readonly ConditionalWeakTable<IShapeWithStrokeLinecap, Data> shapes = [];

    extension (IShapeWithStrokeLinecap shape) {
        public Linecap StrokeLinecap {
            get => shapes.GetOrCreateValue(shape).StrokeLinecap;
            set => shapes.GetOrCreateValue(shape).StrokeLinecap = value;
        }
    }
}
