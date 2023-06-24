namespace BlazorPlayground.Chart.Shapes;

public abstract class Shape {
    public abstract string CssClass { get; }
    public abstract string ElementName { get; }

    public abstract ShapeAttributeCollection GetAttributes();
}
