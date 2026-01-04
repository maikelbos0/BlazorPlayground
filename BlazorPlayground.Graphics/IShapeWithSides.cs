using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithSides { }

    public static class ShapeWithSidesExtensions {
        private class Data {
            public int Sides { get; set; } = DrawSettings.DefaultSides;
        }

        private static readonly ConditionalWeakTable<IShapeWithSides, Data> shapes = [];

        extension(IShapeWithSides shape) {
            public int Sides {
                get => shapes.GetOrCreateValue(shape).Sides;
                set => shapes.GetOrCreateValue(shape).Sides = Math.Max(value, DrawSettings.MinimumSides);
            }
        }
    }
}
