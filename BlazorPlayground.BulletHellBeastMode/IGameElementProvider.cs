using System.Threading.Tasks;

namespace BlazorPlayground.BulletHellBeastMode;

public interface IGameElementProvider {
    Task<TGameElement> CreateFromAsset<TGameElement>(string assetName, Coordinate position) where TGameElement : IGameElement<TGameElement>;
}
