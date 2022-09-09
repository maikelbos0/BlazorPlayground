using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class LineTests {
        [Fact]
        public void ElementName() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            Assert.Equal("line", line.ElementName);
        }

        [Fact]
        public void GetSnapPoints() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            var result = line.GetSnapPoints();

            Assert.Equal(2, result.Count);
            PointAssert.Contains(result, new Point(100, 150));
            PointAssert.Contains(result, new Point(200, 250));
        }

        [Fact]
        public void GetAttributes() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            var attributes = line.GetAttributes().ToList();

            Assert.Equal(4, attributes.Count);
            Assert.Equal("100", Assert.Single(attributes, a => a.Key == "x1").Value);
            Assert.Equal("150", Assert.Single(attributes, a => a.Key == "y1").Value);
            Assert.Equal("200", Assert.Single(attributes, a => a.Key == "x2").Value);
            Assert.Equal("250", Assert.Single(attributes, a => a.Key == "y2").Value);
        }

        [Fact]
        public void Anchors_Get() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            var result = line.Anchors;

            Assert.Equal(2, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0].Get(line));
            PointAssert.Equal(new Point(200, 250), result[1].Get(line));
        }

        [Fact]
        public void Anchors_Set() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            var result = line.Anchors;

            Assert.Equal(2, result.Count);
            result[0].Set(line, new Point(110, 160));
            result[1].Set(line, new Point(210, 260));
            PointAssert.Equal(new Point(110, 160), line.StartPoint);
            PointAssert.Equal(new Point(210, 260), line.EndPoint);
        }

        [Fact]
        public void Clone() {
            var line = new Line(new Point(100, 150), new Point(200, 250));

            var result = line.Clone();

            var resultLine = Assert.IsType<Line>(result);

            Assert.NotSame(line, resultLine);
            PointAssert.Equal(new Point(100, 150), resultLine.StartPoint);
            PointAssert.Equal(new Point(200, 250), resultLine.EndPoint);
        }
    }
}
