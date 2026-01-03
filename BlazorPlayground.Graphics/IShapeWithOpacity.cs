using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithOpacity { }

    public static class ShapeWithOpacityExtensions {
        private class Data {
            public int Opacity { get; set; } = DrawSettings.DefaultOpacity;
        }

        private static readonly ConditionalWeakTable<IShapeWithOpacity, Data> shapes = [];

        extension(IShapeWithOpacity shape) {
            public int Opacity {
                get => shapes.GetOrCreateValue(shape).Opacity;
                set => shapes.GetOrCreateValue(shape).Opacity = Math.Max(Math.Min(value, DrawSettings.MaximumOpacity), DrawSettings.MinimumOpacity);
            }
        }
    }
}
