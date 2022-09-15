using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithOpacity { }

    public static class ShapeWithOpacity {
        private class Data {
            public int Opacity { get; set; } = DrawSettings.DefaultOpacity;
        }

        private static readonly ConditionalWeakTable<IShapeWithOpacity, Data> shapes = new();

        public static void SetOpacity(this IShapeWithOpacity shape, int opacity) => shapes.GetOrCreateValue(shape).Opacity = Math.Max(Math.Min(opacity, DrawSettings.MaximumOpacity), DrawSettings.MinimumOpacity);

        public static int GetOpacity(this IShapeWithOpacity shape) => shapes.GetOrCreateValue(shape).Opacity;
    }
}
