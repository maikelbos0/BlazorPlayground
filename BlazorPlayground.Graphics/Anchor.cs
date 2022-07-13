namespace BlazorPlayground.Graphics {
    public class Anchor {
        private readonly Func<Shape, Point> get;
        private readonly Action<Shape, Point> set;

        protected Anchor(Func<Shape, Point> get, Action<Shape, Point> set) {
            this.get = get;
            this.set = set;
        }

        public void Set(Shape shape, Point point) => set(shape, point);

        public Point Get(Shape shape) => get(shape);

        public void Move(Shape shape, Point delta) => set(shape, get(shape) + delta);
    }

    public class Anchor<TShape> : Anchor where TShape : Shape {
        public Anchor(Func<TShape, Point> get, Action<TShape, Point> set) : base(shape => get((TShape)shape), (shape, point) => set((TShape)shape, point)) { }
    }
}
