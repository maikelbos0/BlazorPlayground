namespace BlazorPlayground.Graphics {
    public record SvgFileParseResult {
        public List<Shape> Shapes { get; }
        public string? ErrorMessage { get; }
        public bool IsSuccess => string.IsNullOrWhiteSpace(ErrorMessage);

        public SvgFileParseResult(string errorMessage) {
            Shapes = new List<Shape>();
            ErrorMessage = errorMessage;
        }

        public SvgFileParseResult(List<Shape> shapes) {
            Shapes = shapes;
        }
    }
}
