using System.Collections.ObjectModel;

namespace BlazorPlayground.Graphics {
    public class ShapeDefinition {
        public delegate Shape Constructor(Point startPoint, Point endPoint);

        public static IEnumerable<ShapeDefinition> Values { get; } = new ReadOnlyCollection<ShapeDefinition>(new List<ShapeDefinition>() {
            new("Line", (startPoint, endPoint) => new Line(startPoint, endPoint), useStrokeLinecap: true),
            new("Rectangle", (startPoint, endPoint) => new Rectangle(startPoint, endPoint), useFill: true, useStrokeLinejoin: true),
            new("Circle", (startPoint, endPoint) => new Circle(startPoint, endPoint), useFill: true),
            new("Ellipse", (startPoint, endPoint) => new Ellipse(startPoint, endPoint), useFill: true),
            new("Regular polygon", (startPoint, endPoint) => new RegularPolygon(startPoint, endPoint), useFill: true, useStrokeLinejoin: true, useSides: true),
            new("Quadratic bezier", (startPoint, endPoint) => new QuadraticBezier(startPoint, endPoint), useFill: true, useStrokeLinecap: true, autoSelect: true),
            new("Cubic bezier", (startPoint, endPoint) => new CubicBezier(startPoint, endPoint), useFill: true, useStrokeLinecap: true, autoSelect: true)
        });

        public string Name { get; }
        public Constructor Construct { get; }
        public bool UseFill { get; }
        public bool UseStrokeLinecap { get; }
        public bool UseStrokeLinejoin { get; }
        public bool UseSides { get; }
        public bool AutoSelect { get; }

        private ShapeDefinition(string name, Constructor construct, bool useFill = false, bool useStrokeLinecap = false, bool useStrokeLinejoin = false, bool useSides = false, bool autoSelect = false) {
            Name = name;
            Construct = construct;
            UseFill = useFill;
            UseStrokeLinecap = useStrokeLinecap;
            UseStrokeLinejoin = useStrokeLinejoin;
            UseSides = useSides;
            AutoSelect = autoSelect;
        }
    }
}
