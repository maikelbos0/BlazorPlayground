using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithFill { }

    public static class ShapeWithFill {
        private class Data {
            public IPaintServer Fill { get; set; } = PaintServer.None;
        }

        private static readonly ConditionalWeakTable<IShapeWithFill, Data> shapes = new();

        public static void SetFill(this IShapeWithFill shape, IPaintServer fill) => shapes.GetOrCreateValue(shape).Fill = fill;

        public static IPaintServer GetFill(this IShapeWithFill shape) => shapes.GetOrCreateValue(shape).Fill;
    }
}
