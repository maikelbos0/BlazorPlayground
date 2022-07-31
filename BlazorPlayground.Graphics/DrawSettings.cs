namespace BlazorPlayground.Graphics {
    public class DrawSettings {
        private int strokeWidth = 1;
        private int sides = 3;

        public PaintManager FillPaintManager { get; set; } = new PaintManager() { Mode = PaintMode.None, ColorValue = "#FFFFFF" };

        public PaintManager StrokePaintManager { get; set; } = new PaintManager() { Mode = PaintMode.Color, ColorValue = "#000000" };

        public Linecap StrokeLinecap { get; set; } = Linecap.Butt;

        public Linejoin StrokeLinejoin { get; set; } = Linejoin.Miter;

        public int StrokeWidth {
            get => strokeWidth;
            set => strokeWidth = Math.Max(value, 1);
        }

        public int Sides {
            get => sides;
            set => sides = Math.Max(value, 3);
        }
    }
}
