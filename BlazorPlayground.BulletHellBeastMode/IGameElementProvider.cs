using System.Threading.Tasks;

namespace BlazorPlayground.BulletHellBeastMode;

public interface IGameElementProvider {
    Task<Ship> CreateShip(Coordinate position);
}
