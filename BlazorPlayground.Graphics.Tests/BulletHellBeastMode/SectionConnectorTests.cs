using BlazorPlayground.Graphics.BulletHellBeastMode;
using System.Collections.Generic;
using Xunit;
using Coordinate = BlazorPlayground.BulletHellBeastMode.Vector<BlazorPlayground.BulletHellBeastMode.CoordinateType>;

namespace BlazorPlayground.Graphics.Tests.BulletHellBeastMode;

public class SectionConnectorTests {
    [Theory]
    [InlineData(true, false, 0)]
    [InlineData(true, true, 2.8)]
    [InlineData(false, true, 5.7)]
    [InlineData(false, false, 2.8)]
    public void GetMagnitude(bool isAppend, bool isReversed, double expectedResult) {
        var currentSection = new List<Coordinate>() {
            new(0, 1),
            new(1, 2),
            new(2, 3)
        };
        var nextSection = new List<Coordinate>() {
            new(2, 3),
            new(3, 4),
            new(4, 5)
        };
        var subject = new SectionConnector(isAppend, isReversed);
        var result = subject.GetMagnitude(currentSection, nextSection);

        Assert.Equal(expectedResult, result, 1);
    }

    [Theory]
    [InlineData(true, false, new double[] { 0, 1, 2, 3, 4, 5 }, new double[] { 1, 2, 3, 4, 5, 6 })]
    [InlineData(true, true, new double[] { 0, 1, 2, 5, 4, 3}, new double[] { 1, 2, 3, 6, 5, 4 })]
    [InlineData(false, false, new double[] { 3, 4, 5, 0, 1, 2 }, new double[] { 4, 5, 6, 1, 2, 3 })]
    [InlineData(false, true, new double[] { 5, 4, 3, 0, 1, 2 }, new double[] { 6, 5, 4, 1, 2, 3 })]
    public void Add(bool isAppend, bool isReversed, double[] expectedXValues, double[] expectedYValues) {
        var path = new List<Coordinate>() {
            new(0, 1),
            new(1, 2),
            new(2, 3)
        };
        var nextSection = new List<Coordinate>() {
            new(3, 4),
            new(4, 5),
            new(5, 6)
        };

        var subject = new SectionConnector(isAppend, isReversed);

        subject.Add(path, nextSection);

        Assert.Equal(expectedXValues.Length, path.Count);
        for (int i = 0; i < path.Count; i++) {
            Assert.Equal(expectedXValues[i], path[i].X);
            Assert.Equal(expectedYValues[i], path[i].Y);
        }
    }

    [Theory]
    [InlineData(true, false, new double[] { 0, 1, 2, 3, 0 }, new double[] { 1, 2, 3, 4, 1 })]
    [InlineData(false, false, new double[] { 2, 3, 0, 1, 2 }, new double[] { 3, 4, 1, 2, 3 })]
    public void Add_Skips_Duplicate_Join_Points(bool isAppend, bool isReversed, double[] expectedXValues, double[] expectedYValues) {
        var path = new List<Coordinate>() {
            new(0, 1),
            new(1, 2),
            new(2, 3)
        };
        var nextSection = new List<Coordinate>() {
            new(2, 3),
            new(3, 4),
            new(0, 1)
        };
        var subject = new SectionConnector(isAppend, isReversed);

        subject.Add(path, nextSection);

        Assert.Equal(expectedXValues.Length, path.Count);
        for (int i = 0; i < path.Count; i++) {
            Assert.Equal(expectedXValues[i], path[i].X);
            Assert.Equal(expectedYValues[i], path[i].Y);
        }
    }
}
