using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public class Canvas {
        private int width = 800;
        private int height = 800;
        private int gridSize = 50;
        private ShapeDefinition currentShapeDefinition = ShapeDefinition.Values.First();

        public int Width {
            get => width;
            set => width = Math.Max(value, 1);
        }
        public int Height {
            get => height;
            set => height = Math.Max(value, 1);
        }
        public int GridSize {
            get => gridSize;
            set => gridSize = Math.Max(value, 1);
        }
        public bool ShowGrid { get; set; } = false;
        public bool SnapToGrid { get; set; } = false;
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
        public Shape? SelectedShape { get; set; }
        public Anchor? SelectedAnchor { get; set; }

        private Point? Snap(Point? point) {
            if (!SnapToGrid || point == null) {
                return point;
            }

            return point.SnapToGrid(GridSize);
        }

        public void StartDrawing(ShapeDefinition shapeDefinition) {
            SelectedShape = null;
            IsDrawing = true;
            CurrentShapeDefinition = shapeDefinition;
        }

        public void StopDrawing() {
            IsDrawing = false;
        }

        public void AddShape() {
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
            if (SnappedStartPoint == null || SnappedEndPoint == null) {
                return null;
            }

            var shape = CurrentShapeDefinition.Construct(SnappedStartPoint, SnappedEndPoint);

            shape.Fill = DrawSettings.FillPaintManager.Server;
            shape.Stroke = DrawSettings.StrokePaintManager.Server;
            shape.StrokeWidth = DrawSettings.StrokeWidth;
            shape.StrokeLinecap = DrawSettings.StrokeLinecap;
            shape.StrokeLinejoin = DrawSettings.StrokeLinejoin;
            shape.Sides = DrawSettings.Sides;

            return shape;
        }

        public Shape CreateVirtualSelectedShape() {
            if (SelectedShape == null) {
                throw new InvalidOperationException($"{nameof(CreateVirtualSelectedShape)} can only be called when {nameof(SelectedShape)} has a value.");
            }

            if (Delta == null || SnappedEndPoint == null) {
                throw new InvalidOperationException($"{nameof(CreateVirtualSelectedShape)} can only be called when {nameof(IsExecutingAction)} is true.");
            }

            var virtualShape = SelectedShape.Clone();

            if (SelectedAnchor == null) {
                virtualShape.Transform(Delta, SnapToGrid, GridSize);
            }
            else {
                SelectedAnchor.Set(virtualShape, SnappedEndPoint);
            }

            return virtualShape;
        }

        public void TransformSelectedShape() {
            if (SelectedShape != null && Delta != null) {
                SelectedShape.Transform(Delta, SnapToGrid, GridSize);
            }

            ClearActionExecution();
        }

        public void TransformSelectedShapeAnchor() {
            if (SelectedShape != null && SelectedAnchor != null && SnappedEndPoint != null) {
                SelectedAnchor.Set(SelectedShape, SnappedEndPoint);
            }

            ClearActionExecution();
        }

        public void DeleteSelectedShape() {
            if (SelectedShape != null) {
                Shapes.Remove(SelectedShape);
                SelectedShape = null;
            }
        }

        public void ClearActionExecution() {
            StartPoint = null;
            EndPoint = null;
            SelectedAnchor = null;
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
