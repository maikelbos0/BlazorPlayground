namespace BlazorPlayground.Graphics {
    public class DrawSettings {
        public const int DefaultOpacity = 100;
        public const int MinimumOpacity = 0;
        public const int MaximumOpacity = 100;
        public const string DefaultFillColor = "#FFFFFF";
        public const string DefaultStrokeColor = "#000000";
        public const int DefaultStrokeWidth = 1;
        public const int MinimumStrokeWidth = 1;
        public const Linecap DefaultStrokeLinecap = Linecap.Butt;
        public const Linejoin DefaultStrokeLinejoin = Linejoin.Miter;
        public const int DefaultSides = 6;
        public const int MinimumSides = 3;

        private int opacity = DefaultOpacity;
        private int fillOpacity = DefaultOpacity;
        private int strokeOpacity = DefaultOpacity;
        private int strokeWidth = DefaultStrokeWidth;
        private int sides = DefaultSides;

        public int Opacity {
            get => opacity;
            set => opacity = Math.Max(Math.Min(value, MaximumOpacity), MinimumOpacity);
        }

        public PaintManager FillPaintManager { get; set; } = new PaintManager() { Mode = PaintMode.None, ColorValue = DefaultFillColor };

        public int FillOpacity {
            get => fillOpacity;
            set => fillOpacity = Math.Max(Math.Min(value, MaximumOpacity), MinimumOpacity);
        }

        public PaintManager StrokePaintManager { get; set; } = new PaintManager() { Mode = PaintMode.Color, ColorValue = DefaultStrokeColor };

        public int StrokeWidth {
            get => strokeWidth;
            set => strokeWidth = Math.Max(value, MinimumStrokeWidth);
        }

        public int StrokeOpacity {
            get => strokeOpacity;
            set => strokeOpacity = Math.Max(Math.Min(value, MaximumOpacity), MinimumOpacity);
        }

        public Linecap StrokeLinecap { get; set; } = DefaultStrokeLinecap;

        public Linejoin StrokeLinejoin { get; set; } = DefaultStrokeLinejoin;

        public int Sides {
            get => sides;
            set => sides = Math.Max(value, MinimumSides);
        }
    }
}
