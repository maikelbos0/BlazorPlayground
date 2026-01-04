using System;
using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics {
    public interface IShapeWithFill { }

    public static class ShapeWithFillExtensions {
        private class Data {
            public IPaintServer Fill { get; set; } = PaintServer.None;
            public int FillOpacity { get; set; } = DrawSettings.DefaultOpacity;
        }

        private static readonly ConditionalWeakTable<IShapeWithFill, Data> shapes = [];

        extension(IShapeWithFill shape) {
            public IPaintServer Fill {
                get => shapes.GetOrCreateValue(shape).Fill;
                set => shapes.GetOrCreateValue(shape).Fill = value;
            }

            public int FillOpacity {
                get => shapes.GetOrCreateValue(shape).FillOpacity;
                set => shapes.GetOrCreateValue(shape).FillOpacity = Math.Clamp(value, DrawSettings.MinimumOpacity, DrawSettings.MaximumOpacity);
            }
        }
    }
}
