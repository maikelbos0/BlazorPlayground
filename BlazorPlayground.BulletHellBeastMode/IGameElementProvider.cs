
namespace BlazorPlayground.BulletHellBeastMode;

public interface IGameElementProvider {
    Task<GameElement> LoadFromAsset(string assetName);
}
