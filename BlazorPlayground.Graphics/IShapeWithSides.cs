using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithSides { }

    public static class ShapeWithSides {
        private class Data {
            public int Sides { get; set; } = DrawSettings.DefaultSides;
        }

        private static readonly ConditionalWeakTable<IShapeWithSides, Data> shapes = new();

        public static void SetSides(this IShapeWithSides shape, int sides) => shapes.GetOrCreateValue(shape).Sides = Math.Max(sides, DrawSettings.MinimumSides);

        public static int GetSides(this IShapeWithSides shape) => shapes.GetOrCreateValue(shape).Sides;
    }
}
