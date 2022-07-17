namespace BlazorPlayground.Graphics {
    public class Color {
        public string ColorValue { get; }
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public double Alpha { get; }

        public Color(string colorValue, byte red, byte green, byte blue, double alpha) {
            ColorValue = colorValue;
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public override string ToString() => ColorValue;

        public static implicit operator Color(string colorValue) {
            var color = System.Drawing.Color.FromName(colorValue);

            return new Color(colorValue, color.R, color.G, color.B, color.A / 255.0);
        }
    }
}
