namespace BlazorPlayground.Graphics {
    public record Color(byte Red, byte Green, byte Blue, double Alpha) : IPaintServer {
        public Color ContrastingColor => (Red + Green + Blue) * Alpha + 255 * 3 * (1 - Alpha) > 381 ? new Color(0, 0, 0, 1) : new Color(255, 255, 255, 1);

        public override string ToString() => Alpha == 1 ? FormattableString.Invariant($"#{Red:X2}{Green:X2}{Blue:X2}") : FormattableString.Invariant($"rgba({Red}, {Green}, {Blue}, {Alpha})");
    }
}
