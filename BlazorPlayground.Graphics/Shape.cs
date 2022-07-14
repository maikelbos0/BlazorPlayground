namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public abstract ShapeRenderType RenderType { get; }
        public abstract IReadOnlyList<Anchor> Anchors { get; }

        public abstract IEnumerable<Point> GetPoints();

        public abstract Shape Clone();

        public void Transform(Point delta, bool snapToGrid, int gridSize) {
            if (snapToGrid) {
                var point = Anchors
                    .Select(a => a.Get(this))
                    .Select(p => new { Original = p, Transformed = p + delta, Anchored = (p + delta).SnapToGrid(gridSize) })
                    .OrderBy(p => (p.Transformed - p.Anchored).Distance)
                    .First();

                delta = point.Anchored - point.Original;
            }

            foreach (var anchor in Anchors) {
                anchor.Move(this, delta);
            }
        }
    }
}
