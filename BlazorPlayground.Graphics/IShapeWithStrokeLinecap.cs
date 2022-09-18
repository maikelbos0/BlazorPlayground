using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithStrokeLinecap { }

    public static class ShapeWithStrokeLinecap {
        private class Data {
            public Linecap StrokeLinecap { get; set; }
        }

        private static readonly ConditionalWeakTable<IShapeWithStrokeLinecap, Data> shapes = new();


        public static void SetStrokeLinecap(this IShapeWithStrokeLinecap shape, Linecap strokeLinecap) => shapes.GetOrCreateValue(shape).StrokeLinecap = strokeLinecap;

        public static Linecap GetStrokeLinecap(this IShapeWithStrokeLinecap shape) => shapes.GetOrCreateValue(shape).StrokeLinecap;
    }
}
