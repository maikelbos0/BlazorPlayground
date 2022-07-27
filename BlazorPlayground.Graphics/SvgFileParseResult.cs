namespace BlazorPlayground.Graphics {
    public record SvgFileParseResult {
        private readonly Canvas? canvas;

        public Canvas Canvas => canvas ?? throw new InvalidOperationException($"{nameof(Canvas)} is only available when {nameof(IsSuccess)} is true.");
        public bool IsSuccess => canvas != null;
        public string? ErrorMessage { get; }

        public SvgFileParseResult(string errorMessage) {
            ErrorMessage = errorMessage;
        }

        public SvgFileParseResult(Canvas canvas) {
            this.canvas = canvas;
        }
    }
}
