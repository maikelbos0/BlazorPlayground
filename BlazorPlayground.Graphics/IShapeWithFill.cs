using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithFill { }

    public static class ShapeWithFill {
        private class Data {
            public IPaintServer Fill { get; set; } = PaintServer.None;
            public int FillOpacity { get; set; } = DrawSettings.DefaultOpacity;
        }

        private static readonly ConditionalWeakTable<IShapeWithFill, Data> shapes = new();

        public static void SetFill(this IShapeWithFill shape, IPaintServer fill) => shapes.GetOrCreateValue(shape).Fill = fill;

        public static IPaintServer GetFill(this IShapeWithFill shape) => shapes.GetOrCreateValue(shape).Fill;

        public static void SetFillOpacity(this IShapeWithFill shape, int fillOpacity) => shapes.GetOrCreateValue(shape).FillOpacity = Math.Max(Math.Min(fillOpacity, DrawSettings.MaximumOpacity), DrawSettings.MinimumOpacity);

        public static int GetFillOpacity(this IShapeWithFill shape) => shapes.GetOrCreateValue(shape).FillOpacity;
    }
}
