
namespace BlazorPlayground.BulletHellBeastMode;

public interface IGameElementProvider {
    Task<GameElement> CreateFromAsset(string assetName);
}
