namespace BlazorPlayground.Graphics {
    public class DrawSettings {
        public const int DefaultStrokeWidth = 1;
        public const int MinimumStrokeWidth = 1;
        public const Linecap DefaultStrokeLinecap = Linecap.Butt;
        public const Linejoin DefaultStrokeLinejoin = Linejoin.Miter;
        public const int DefaultSides = 6;
        public const int MinimumSides = 3;
        public const string DefaultFillColor = "#FFFFFF";
        public const string DefaultStrokeColor = "#000000";

        private int strokeWidth = DefaultStrokeWidth;
        private int sides = DefaultSides;

        public PaintManager FillPaintManager { get; set; } = new PaintManager() { Mode = PaintMode.None, ColorValue = DefaultFillColor };

        public PaintManager StrokePaintManager { get; set; } = new PaintManager() { Mode = PaintMode.Color, ColorValue = DefaultStrokeColor };

        public Linecap StrokeLinecap { get; set; } = DefaultStrokeLinecap;

        public Linejoin StrokeLinejoin { get; set; } = DefaultStrokeLinejoin;

        public int StrokeWidth {
            get => strokeWidth;
            set => strokeWidth = Math.Max(value, MinimumStrokeWidth);
        }

        public int Sides {
            get => sides;
            set => sides = Math.Max(value, MinimumSides);
        }
    }
}
