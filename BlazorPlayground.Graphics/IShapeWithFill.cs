using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithFill { }

    public static class ShapeWithFill {

        private static readonly ConditionalWeakTable<IShapeWithFill, IPaintServer> shapes = new();

        public static void SetFill(this IShapeWithFill shape, IPaintServer fill) => shapes.AddOrUpdate(shape, fill);

        public static IPaintServer GetFill(this IShapeWithFill shape) => shapes.TryGetValue(shape, out var fill) ? fill : PaintServer.None;
    }
}
