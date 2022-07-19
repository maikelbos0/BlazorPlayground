namespace BlazorPlayground.Graphics {
    public class PaintServer : IPaintServer {
        private readonly string value;

        public static PaintServer None { get; } = new PaintServer("none");

        private PaintServer(string value) {
            this.value = value;
        }

        public override string ToString() => value;
    }
}
