namespace BlazorPlayground.Graphics {
    public class DrawSettings {
        public const double DefaultOpacity = 1;
        public const double MinimumOpacity = 0;
        public const double MaximumOpacity = 1;
        public const string DefaultFillColor = "#FFFFFF";
        public const string DefaultStrokeColor = "#000000";
        public const int DefaultStrokeWidth = 1;
        public const int MinimumStrokeWidth = 1;
        public const Linecap DefaultStrokeLinecap = Linecap.Butt;
        public const Linejoin DefaultStrokeLinejoin = Linejoin.Miter;
        public const int DefaultSides = 6;
        public const int MinimumSides = 3;

        private double opacity = DefaultOpacity;
        private int strokeWidth = DefaultStrokeWidth;
        private int sides = DefaultSides;

        public double Opacity {
            get => opacity;
            set => opacity = Math.Max(Math.Min(value, MaximumOpacity), MinimumOpacity);
        }

        public PaintManager FillPaintManager { get; set; } = new PaintManager() { Mode = PaintMode.None, ColorValue = DefaultFillColor };

        public PaintManager StrokePaintManager { get; set; } = new PaintManager() { Mode = PaintMode.Color, ColorValue = DefaultStrokeColor };

        public int StrokeWidth {
            get => strokeWidth;
            set => strokeWidth = Math.Max(value, MinimumStrokeWidth);
        }

        public Linecap StrokeLinecap { get; set; } = DefaultStrokeLinecap;

        public Linejoin StrokeLinejoin { get; set; } = DefaultStrokeLinejoin;

        public int Sides {
            get => sides;
            set => sides = Math.Max(value, MinimumSides);
        }
    }
}
