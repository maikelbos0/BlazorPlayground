namespace BlazorPlayground.Graphics {
    public class Anchor {
        private readonly Func<Point> get;
        private readonly Action<Point> set;

        protected Anchor(Func<Point> get, Action<Point> set) {
            this.get = get;
            this.set = set;
        }

        // TODO test below
        public void Set(Point point) => set(point);

        public Point Get() => get();
    }

    public class Anchor<TShape> : Anchor where TShape : Shape {
        public Anchor(TShape shape, Func<TShape, Point> get, Action<TShape, Point> set) : base(() => get(shape), point => set(shape, point)) { }
    }
}
