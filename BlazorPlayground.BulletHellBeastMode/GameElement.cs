namespace BlazorPlayground.BulletHellBeastMode;

public class GameElement {
    public required Coordinate Position { get; set; }
    public required List<GameElementSection> Sections { get; set; }

    public CanvasGameElement ForCanvas() => new(Sections.Select(section => section.ForCanvas()).ToList());
}
