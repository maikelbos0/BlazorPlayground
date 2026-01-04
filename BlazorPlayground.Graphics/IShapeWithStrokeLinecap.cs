using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithStrokeLinecap { }

    public static class ShapeWithStrokeLinecapExtensions {
        private class Data {
            public Linecap StrokeLinecap { get; set; }
        }

        private static readonly ConditionalWeakTable<IShapeWithStrokeLinecap, Data> shapes = new();

        extension (IShapeWithStrokeLinecap shape) {
            public Linecap StrokeLinecap {
                get => shapes.GetOrCreateValue(shape).StrokeLinecap;
                set => shapes.GetOrCreateValue(shape).StrokeLinecap = value;
            }
        }
    }
}
