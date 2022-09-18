namespace BlazorPlayground.Graphics {
    public class ShapeDefinition {
        public delegate Shape Constructor(Point startPoint, Point endPoint);

        private readonly static Dictionary<Type, ShapeDefinition> definitions = new() {
            { typeof(Line), new(typeof(Line), "Line", (startPoint, endPoint) => new Line(startPoint, endPoint)) },
            { typeof(Rectangle), new(typeof(Rectangle), "Rectangle", (startPoint, endPoint) => new Rectangle(startPoint, endPoint)) },
            { typeof(Circle), new(typeof(Circle), "Circle", (startPoint, endPoint) => new Circle(startPoint, endPoint)) },
            { typeof(Ellipse), new(typeof(Ellipse), "Ellipse", (startPoint, endPoint) => new Ellipse(startPoint, endPoint)) },
            { typeof(RegularPolygon), new(typeof(RegularPolygon), "Regular polygon", (startPoint, endPoint) => new RegularPolygon(startPoint, endPoint)) },
            { typeof(QuadraticBezier), new(typeof(QuadraticBezier), "Quadratic bezier", (startPoint, endPoint) => new QuadraticBezier(startPoint, endPoint)) },
            { typeof(CubicBezier), new(typeof(CubicBezier), "Cubic bezier", (startPoint, endPoint) => new CubicBezier(startPoint, endPoint)) },
            { typeof(RawShape), new(typeof(RawShape), "Raw shape") }
        };

        public static ShapeDefinition None { get; } = new ShapeDefinition(null, "None");

        public static IEnumerable<ShapeDefinition> Values => definitions.Values;

        public static ShapeDefinition Get(Shape shape) => Get(shape.GetType());

        public static ShapeDefinition Get(Type type) => definitions[type];

        public Type? Type { get; }
        public string Name { get; }
        public bool IsConstructable { get; }
        public Constructor Construct { get; }
        public bool UseOpacity { get; }
        public bool UseFill { get; }
        public bool UseStroke { get; }
        public bool UseStrokeLinecap { get; }
        public bool UseStrokeLinejoin { get; }
        public bool UseSides { get; }
        public bool AutoSelect { get; }

        private ShapeDefinition(Type? type, string name, Constructor? construct = null) {
            Type = type;
            Name = name;
            IsConstructable = construct != null;
            Construct = construct ?? ((_, _) => throw new InvalidOperationException($"{nameof(Construct)} can only be called when {nameof(IsConstructable)} is true."));
            UseOpacity = typeof(IShapeWithOpacity).IsAssignableFrom(type);
            UseFill = typeof(IShapeWithFill).IsAssignableFrom(type);
            UseStroke = typeof(IShapeWithStroke).IsAssignableFrom(type);
            UseStrokeLinecap = typeof(IShapeWithStrokeLinecap).IsAssignableFrom(type);
            UseStrokeLinejoin = typeof(IShapeWithStrokeLinejoin).IsAssignableFrom(type); ;
            UseSides = typeof(IShapeWithSides).IsAssignableFrom(type);
            AutoSelect = typeof(IAutoSelectedShape).IsAssignableFrom(type);
        }
    }
}
