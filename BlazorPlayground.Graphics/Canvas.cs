using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public class Canvas {
        public const int DefaultWidth = 800;
        public const int MinimumWidth = 100;
        public const int DefaultHeight = 800;
        public const int MinimumHeight = 100;
        public const int DefaultGridSize = 50;
        public const int MinimumGridSize = 1;

        private int width = DefaultWidth;
        private int height = DefaultHeight;
        private int gridSize = DefaultGridSize;
        private ShapeDefinition currentShapeDefinition = ShapeDefinition.Values.First();

        public int Width {
            get => width;
            set => width = Math.Max(value, MinimumWidth);
        }
        public int Height {
            get => height;
            set => height = Math.Max(value, MinimumHeight);
        }
        public int GridSize {
            get => gridSize;
            set => gridSize = Math.Max(value, MinimumGridSize);
        }
        public bool ShowGrid { get; set; } = false;
        public bool SnapToGrid { get; set; } = false;
        public bool SnapToShapes { get; set; } = false;
        public List<Shape> Shapes { get; set; } = new List<Shape>();
        public Point? StartPoint { get; set; }
        public Point? SnappedStartPoint => Snap(StartPoint);
        public Point? EndPoint { get; set; }
        public Point? SnappedEndPoint => Snap(EndPoint);
        public bool IsExecutingAction => StartPoint != null && EndPoint != null;
        public Point? Delta => StartPoint != null && EndPoint != null ? EndPoint - StartPoint : null;
        public DrawSettings DrawSettings { get; } = new DrawSettings();
        public bool IsDrawing { get; internal set; } = true;

        public ShapeDefinition CurrentShapeDefinition {
            get {
                if (IsDrawing) {
                    return currentShapeDefinition;
                }
                else if (SelectedShape != null) {
                    return ShapeDefinition.Get(SelectedShape);
                }
                else {
                    return ShapeDefinition.None;
                }
            }
            set => currentShapeDefinition = value;
        }
        public Shape? SelectedShape { get; internal set; }
        public bool IsSelecting { get; internal set; } = false;
        public Anchor? SelectedAnchor { get; internal set; }

        public Point? Snap(Point? point) {
            if (point == null) {
                return point;
            }

            return point.Snap(SnapToGrid, GridSize, SnapToShapes, GetSnapPoints());
        }

        public IEnumerable<Shape> GetStaticShapes() => Shapes.Where(s => SelectedShape == null || s != SelectedShape);

        public HashSet<Point> GetSnapPoints() {
            var snapPoints = new HashSet<Point>();

            if (SnapToShapes) {
                foreach (var shape in GetStaticShapes()) {
                    snapPoints.UnionWith(shape.GetSnapPoints());
                }
            }

            return snapPoints;
        }

        public void StartDrawing(ShapeDefinition shapeDefinition) {
            SelectedShape = null;
            IsDrawing = true;
            CurrentShapeDefinition = shapeDefinition;
        }

        public void StopDrawing() {
            IsDrawing = false;
        }

        public Shape? CreateVirtualSelectedShape() {
            if (SelectedShape == null || Delta == null || SnappedEndPoint == null) {
                return null;
            }

            var virtualShape = SelectedShape.Clone();

            if (SelectedAnchor == null) {
                virtualShape.Transform(Delta, SnapToGrid, GridSize, SnapToShapes, GetSnapPoints());
            }
            else {
                SelectedAnchor.Set(virtualShape, SnappedEndPoint);
            }

            return virtualShape;
        }

        public void SelectShape(Shape shape) {
            if (!IsDrawing) {
                SelectedShape = shape;
                IsSelecting = true;
            }
        }

        public void SelectAnchor(Anchor anchor) {
            if (!IsDrawing) {
                SelectedAnchor = anchor;
            }
        }

        public void StartActionExecution(Point startPoint) {
            StartPoint = startPoint;
        }

        public void UpdateActionExecution(Point endPoint) {
            if (StartPoint != null) {
                EndPoint = endPoint;
            }
        }

        public void EndActionExecution() {
            if (StartPoint != null && EndPoint != null) {
                if (IsDrawing) {
                    AddShape();
                }
                else if (SelectedAnchor != null) {
                    TransformSelectedShapeAnchor();
                }
                else if (SelectedShape != null) {
                    TransformSelectedShape();
                }
            }
            else if (!IsSelecting) {
                SelectedShape = null;
            }

            StartPoint = null;
            EndPoint = null;
            SelectedAnchor = null;
            IsSelecting = false;
        }

        internal void AddShape() {
            var shape = CreateShape();

            if (shape != null) {
                Shapes.Add(shape);

                if (CurrentShapeDefinition.AutoSelect) {
                    SelectedShape = shape;
                    StopDrawing();
                }
            }
        }

        public Shape? CreateShape() {
            if (!IsDrawing || SnappedStartPoint == null || SnappedEndPoint == null) {
                return null;
            }

            var shape = CurrentShapeDefinition.Construct(SnappedStartPoint, SnappedEndPoint);

            (shape as IShapeWithOpacity)?.SetOpacity(DrawSettings.Opacity);
            (shape as IShapeWithFill)?.SetFill(DrawSettings.FillPaintManager.Server);
            (shape as IShapeWithFill)?.SetFillOpacity(DrawSettings.FillOpacity);
            (shape as IShapeWithStroke)?.SetStroke(DrawSettings.StrokePaintManager.Server);
            (shape as IShapeWithStroke)?.SetStrokeWidth(DrawSettings.StrokeWidth);
            (shape as IShapeWithStroke)?.SetStrokeOpacity(DrawSettings.StrokeOpacity);
            (shape as IShapeWithStrokeLinecap)?.SetStrokeLinecap(DrawSettings.StrokeLinecap);
            (shape as IShapeWithStrokeLinejoin)?.SetStrokeLinejoin(DrawSettings.StrokeLinejoin);
            (shape as IShapeWithSides)?.SetSides(DrawSettings.Sides);

            return shape;
        }

        internal void TransformSelectedShape() {
            if (SelectedShape != null && Delta != null) {
                SelectedShape.Transform(Delta, SnapToGrid, GridSize, SnapToShapes, GetSnapPoints());
            }
        }

        internal void TransformSelectedShapeAnchor() {
            if (SelectedShape != null && SelectedAnchor != null && SnappedEndPoint != null) {
                SelectedAnchor.Set(SelectedShape, SnappedEndPoint);
            }
        }

        public void ApplyOpacityToSelectedShape() {
            (SelectedShape as IShapeWithOpacity)?.SetOpacity(DrawSettings.Opacity);
        }

        public void ApplyFillToSelectedShape() {
            (SelectedShape as IShapeWithFill)?.SetFill(DrawSettings.FillPaintManager.Server);
        }

        public void ApplyFillOpacityToSelectedShape() {
            (SelectedShape as IShapeWithFill)?.SetFillOpacity(DrawSettings.FillOpacity);
        }

        public void ApplyStrokeToSelectedShape() {
            (SelectedShape as IShapeWithStroke)?.SetStroke(DrawSettings.StrokePaintManager.Server);
        }

        public void ApplyStrokeWidthToSelectedShape() {
            (SelectedShape as IShapeWithStroke)?.SetStrokeWidth(DrawSettings.StrokeWidth);
        }

        public void ApplyStrokeOpacityToSelectedShape() {
            (SelectedShape as IShapeWithStroke)?.SetStrokeOpacity(DrawSettings.StrokeOpacity);
        }

        public void ApplyStrokeLinecapToSelectedShape() {
            (SelectedShape as IShapeWithStrokeLinecap)?.SetStrokeLinecap(DrawSettings.StrokeLinecap);
        }

        public void ApplyStrokeLinejoinToSelectedShape() {
            (SelectedShape as IShapeWithStrokeLinejoin)?.SetStrokeLinejoin(DrawSettings.StrokeLinejoin);
        }

        public void ApplySidesToSelectedShape() {
            (SelectedShape as IShapeWithSides)?.SetSides(DrawSettings.Sides);
        }

        public void MoveSelectedShapeToBack() {
            if (SelectedShape != null) {
                Shapes.Remove(SelectedShape);
                Shapes.Insert(0, SelectedShape);
            }
        }

        public void MoveSelectedShapeBackward() {
            if (SelectedShape != null) {
                var index = Shapes.IndexOf(SelectedShape);
                var newIndex = index - 1;

                if (newIndex >= 0 && newIndex < Shapes.Count) {
                    Shapes[index] = Shapes[newIndex];
                    Shapes[newIndex] = SelectedShape;
                }
            }
        }

        public void MoveSelectedShapeForward() {
            if (SelectedShape != null) {
                var index = Shapes.IndexOf(SelectedShape);
                var newIndex = index + 1;

                if (newIndex >= 0 && newIndex < Shapes.Count) {
                    Shapes[index] = Shapes[newIndex];
                    Shapes[newIndex] = SelectedShape;
                }
            }
        }

        public void MoveSelectedShapeToFront() {
            if (SelectedShape != null) {
                Shapes.Remove(SelectedShape);
                Shapes.Add(SelectedShape);
            }
        }

        public void DeleteSelectedShape() {
            if (SelectedShape != null) {
                Shapes.Remove(SelectedShape);
                SelectedShape = null;
            }
        }

        public XElement ExportSvg() => new(
            "svg",
            new XAttribute("viewBox", $"0 0 {Width} {Height}"),
            new XAttribute("width", Width),
            new XAttribute("height", Height),
            Shapes.Select(s => s.CreateSvgElement())
        );
    }
}
