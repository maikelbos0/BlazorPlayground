namespace BlazorPlayground.Graphics {
    public abstract class Shape {
        public Color FillColor { get; set; } = new Color(255, 255, 255, 1);
        public Color StrokeColor { get; set; } = new Color(0, 0, 0, 1);
        public int StrokeWidth { get; set; } = 1;
        public abstract string ElementName { get; }
        public abstract IReadOnlyList<Anchor> Anchors { get; }

        public abstract IEnumerable<ShapeAttribute> GetAttributes();

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
