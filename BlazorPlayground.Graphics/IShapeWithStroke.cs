using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithStroke { }

    public static class ShapeWithStroke {
        private class Data {
            public IPaintServer Stroke { get; set; } = PaintManager.ParseColor(DrawSettings.DefaultStrokeColor);
            public int StrokeWidth { get; set; }
        }

        private static readonly ConditionalWeakTable<IShapeWithStroke, Data> shapes = new();


        public static void SetStroke(this IShapeWithStroke shape, IPaintServer stroke) => shapes.GetOrCreateValue(shape).Stroke = stroke;

        public static IPaintServer GetStroke(this IShapeWithStroke shape) => shapes.GetOrCreateValue(shape).Stroke;

        public static void SetStrokeWidth(this IShapeWithStroke shape, int strokeWidth) => shapes.GetOrCreateValue(shape).StrokeWidth = Math.Max(strokeWidth, DrawSettings.MinimumStrokeWidth);

        public static int GetStrokeWidth(this IShapeWithStroke shape) => shapes.GetOrCreateValue(shape).StrokeWidth;
    }
}
