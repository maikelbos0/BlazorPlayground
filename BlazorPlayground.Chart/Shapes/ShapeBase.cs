namespace BlazorPlayground.Chart.Shapes;

public abstract class ShapeBase {
    public abstract string CssClass { get; }
    public abstract string ElementName { get; }
    public string Key { get; }

    public ShapeBase(params int[] indexes) {
        Key = $"{GetType().Name}[{string.Join(",", indexes)}]";
    }

    public abstract ShapeAttributeCollection GetAttributes();
    public virtual string? GetContent() => null;
}
