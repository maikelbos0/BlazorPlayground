using System;
using System.Runtime.CompilerServices;

namespace BlazorPlayground.Graphics;

public static class ShapeWithStrokeExtensions {
    private class Data {
        public IPaintServer Stroke { get; set; } = PaintManager.ParseColor(DrawSettings.DefaultStrokeColor);
        public int StrokeWidth { get; set; }
        public int StrokeOpacity { get; set; } = DrawSettings.DefaultOpacity;
    }

    private static readonly ConditionalWeakTable<IShapeWithStroke, Data> shapes = [];

    extension(IShapeWithStroke shape) {
        public IPaintServer Stroke {
            get => shapes.GetOrCreateValue(shape).Stroke;
            set => shapes.GetOrCreateValue(shape).Stroke = value;
        }

        public int StrokeWidth {
            get => shapes.GetOrCreateValue(shape).StrokeWidth;
            set => shapes.GetOrCreateValue(shape).StrokeWidth = Math.Max(value, DrawSettings.MinimumStrokeWidth);
        }

        public int StrokeOpacity {
            get => shapes.GetOrCreateValue(shape).StrokeOpacity;
            set => shapes.GetOrCreateValue(shape).StrokeOpacity = Math.Clamp(value, DrawSettings.MinimumOpacity, DrawSettings.MaximumOpacity);
        }
    }
}
