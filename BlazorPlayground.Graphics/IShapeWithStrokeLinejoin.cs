using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithStrokeLinejoin {    }

    public static class ShapeWithStrokeLinejoin {
        private class Data {
            public Linejoin StrokeLinejoin { get; set; }
        }

        private static readonly ConditionalWeakTable<IShapeWithStrokeLinejoin, Data> shapes = new();


        public static void SetStrokeLinejoin(this IShapeWithStrokeLinejoin shape, Linejoin strokeLinejoin) => shapes.GetOrCreateValue(shape).StrokeLinejoin = strokeLinejoin;

        public static Linejoin GetStrokeLinejoin(this IShapeWithStrokeLinejoin shape) => shapes.GetOrCreateValue(shape).StrokeLinejoin;
    }
}
