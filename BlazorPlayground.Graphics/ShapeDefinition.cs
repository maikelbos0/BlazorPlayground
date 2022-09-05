namespace BlazorPlayground.Graphics {
    public class ShapeDefinition {
        public delegate Shape Constructor(Point startPoint, Point endPoint);

        private readonly static Dictionary<Type, ShapeDefinition> definitions = new() {
            { typeof(Line), new("Line", (startPoint, endPoint) => new Line(startPoint, endPoint), useOpacity: true, useStroke: true, useStrokeWidth: true, useStrokeLinecap: true) },
            { typeof(Rectangle), new("Rectangle", (startPoint, endPoint) => new Rectangle(startPoint, endPoint), useOpacity: true, useFill: true, useStroke: true, useStrokeWidth: true, useStrokeLinejoin: true) },
            { typeof(Circle), new("Circle", (startPoint, endPoint) => new Circle(startPoint, endPoint), useOpacity: true, useFill: true, useStroke: true, useStrokeWidth: true) },
            { typeof(Ellipse), new("Ellipse", (startPoint, endPoint) => new Ellipse(startPoint, endPoint), useOpacity: true, useFill: true, useStroke: true, useStrokeWidth: true) },
            { typeof(RegularPolygon), new("Regular polygon", (startPoint, endPoint) => new RegularPolygon(startPoint, endPoint), useOpacity: true, useFill: true, useStroke: true, useStrokeWidth: true, useStrokeLinejoin: true, useSides: true) },
            { typeof(QuadraticBezier), new("Quadratic bezier", (startPoint, endPoint) => new QuadraticBezier(startPoint, endPoint), useOpacity: true, useFill: true, useStroke: true, useStrokeWidth: true, useStrokeLinecap: true, autoSelect: true) },
            { typeof(CubicBezier), new("Cubic bezier", (startPoint, endPoint) => new CubicBezier(startPoint, endPoint), useOpacity: true, useFill: true, useStroke: true, useStrokeWidth: true, useStrokeLinecap: true, autoSelect: true) },
            { typeof(RawShape), new("Raw shape") }
        };

        public static ShapeDefinition None { get; } = new ShapeDefinition("None");

        public static IEnumerable<ShapeDefinition> Values => definitions.Values;

        public static ShapeDefinition Get(Shape shape) => Get(shape.GetType());

        public static ShapeDefinition Get(Type type) => definitions[type];

        public string Name { get; }
        public bool IsConstructable { get; }
        public Constructor Construct { get; }
        public bool UseOpacity { get; }
        public bool UseFill { get; }
        public bool UseStroke { get; }
        public bool UseStrokeWidth { get; }
        public bool UseStrokeLinecap { get; }
        public bool UseStrokeLinejoin { get; }
        public bool UseSides { get; }
        public bool AutoSelect { get; }

        private ShapeDefinition(string name, Constructor? construct = null, bool useOpacity = false, bool useFill = false, bool useStroke = false, bool useStrokeWidth = false, bool useStrokeLinecap = false, bool useStrokeLinejoin = false, bool useSides = false, bool autoSelect = false) {
            Name = name;
            IsConstructable = construct != null;
            Construct = construct ?? ((_, _) => throw new InvalidOperationException($"{nameof(Construct)} can only be called when {nameof(IsConstructable)} is true."));
            UseOpacity = useOpacity;
            UseFill = useFill;
            UseStroke = useStroke;
            UseStrokeWidth = useStrokeWidth;
            UseStrokeLinecap = useStrokeLinecap;
            UseStrokeLinejoin = useStrokeLinejoin;
            UseSides = useSides;
            AutoSelect = autoSelect;
        }
    }
}
