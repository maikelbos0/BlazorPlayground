using System.Collections.Generic;
using System.Linq;
using Coordinate = BlazorPlayground.BulletHellBeastMode.Vector<BlazorPlayground.BulletHellBeastMode.CoordinateType>;

namespace BlazorPlayground.Graphics.BulletHellBeastMode;

public record SectionConnector(bool IsAppend, bool IsReversed) {
    public static IReadOnlyList<SectionConnector> All { get; } = [
        new(true, false),
        new(true, true),
        new(false, true),
        new(false, false)
    ];

    public double GetMagnitude(List<Coordinate> currentSection, List<Coordinate> nextSection)
        => (currentSection[IsAppend ? ^1 : 0] - nextSection[IsAppend == IsReversed ? ^1 : 0]).Magnitude;

    public void Add(List<Coordinate> path, List<Coordinate> nextSection) {
        IEnumerable<Coordinate> coordinatesToAdd = nextSection;

        if (IsReversed) {
            coordinatesToAdd = coordinatesToAdd.Reverse();
        }

        if (IsAppend) {
            if ((path[^1] - coordinatesToAdd.First()).Magnitude == 0) {
                coordinatesToAdd = coordinatesToAdd.Skip(1);
            }

            path.AddRange(coordinatesToAdd);
        }
        else {
            if ((path[0] - coordinatesToAdd.Last()).Magnitude == 0) {
                coordinatesToAdd = coordinatesToAdd.Take(nextSection.Count - 1);
            }

            path.InsertRange(0, coordinatesToAdd);
        }
    }
}
