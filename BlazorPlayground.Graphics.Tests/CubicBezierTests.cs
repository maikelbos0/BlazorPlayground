using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class CubicBezierTests {
        [Fact]
        public void ElementName() {
            var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250));

            Assert.Equal("path", bezier.ElementName);
        }

        [Fact]
        public void GetSnapPoints() {
            var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250));

            var result = bezier.GetSnapPoints();

            Assert.Equal(2, result.Count);
            PointAssert.Contains(result, new Point(100, 150));
            PointAssert.Contains(result, new Point(200, 250));
        }

        [Fact]
        public void ControlPoints() {
            var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250));

            PointAssert.Equal(new Point(133.333, 183.333), bezier.ControlPoint1);
            PointAssert.Equal(new Point(166.667, 216.667), bezier.ControlPoint2);
        }

        [Fact]
        public void GetAttributes() {
            var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250)) {
                ControlPoint1 = new Point(125, 175),
                ControlPoint2 = new Point(225, 275)
            };

            var attributes = bezier.GetAttributes();

            var attribute = Assert.Single(attributes);
            Assert.Equal("d", attribute.Key);
            Assert.Equal("M 100 150 C 125 175, 225 275, 200 250", attribute.Value);
        }

        [Fact]
        public void Anchors_Get() {
            var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250)) {
                ControlPoint1 = new Point(125, 175),
                ControlPoint2 = new Point(225, 275)
            };

            var result = bezier.Anchors;

            Assert.Equal(4, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0].Get(bezier));
            PointAssert.Equal(new Point(125, 175), result[1].Get(bezier));
            PointAssert.Equal(new Point(225, 275), result[2].Get(bezier));
            PointAssert.Equal(new Point(200, 250), result[3].Get(bezier));
        }

        [Fact]
        public void Anchors_Set() {
            var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250)) {
                ControlPoint1 = new Point(125, 175),
                ControlPoint2 = new Point(225, 275)
            };

            var result = bezier.Anchors;

            Assert.Equal(4, result.Count);
            result[0].Set(bezier, new Point(110, 160));
            result[1].Set(bezier, new Point(135, 185));
            result[2].Set(bezier, new Point(235, 285));
            result[3].Set(bezier, new Point(210, 260));
            PointAssert.Equal(new Point(110, 160), bezier.StartPoint);
            PointAssert.Equal(new Point(135, 185), bezier.ControlPoint1);
            PointAssert.Equal(new Point(235, 285), bezier.ControlPoint2);
            PointAssert.Equal(new Point(210, 260), bezier.EndPoint);
        }

        [Fact]
        public void Clone() {
            var bezier = new CubicBezier(new Point(100, 150), new Point(200, 250)) {
                ControlPoint1 = new Point(125, 175),
                ControlPoint2 = new Point(225, 275)
            };

            var result = bezier.Clone();

            var resultBezier = Assert.IsType<CubicBezier>(result);

            Assert.NotSame(bezier, resultBezier);
            PointAssert.Equal(new Point(100, 150), resultBezier.StartPoint);
            PointAssert.Equal(new Point(125, 175), resultBezier.ControlPoint1);
            PointAssert.Equal(new Point(225, 275), resultBezier.ControlPoint2);
            PointAssert.Equal(new Point(200, 250), resultBezier.EndPoint);
        }
    }
}
