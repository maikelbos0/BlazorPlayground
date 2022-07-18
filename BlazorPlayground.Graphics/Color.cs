namespace BlazorPlayground.Graphics {
    public record Color(byte Red, byte Green, byte Blue, double Alpha) {
        public override string ToString() => Alpha == 1 ? FormattableString.Invariant($"#{Red:X2}{Green:X2}{Blue:X2}") : FormattableString.Invariant($"rgba({Red}, {Green}, {Blue}, {Alpha})");
    }
}
