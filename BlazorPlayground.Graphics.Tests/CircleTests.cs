using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class CircleTests {
        [Fact]
        public void ElementName() {
            var circle = new Circle(new Point(100, 150), new Point(200, 250));

            Assert.Equal("circle", circle.ElementName);
        }

        [Fact]
        public void GetSnapPoints() {
            var circle = new Circle(new Point(100, 150), new Point(200, 200));

            var result = circle.GetSnapPoints();

            Assert.Equal(5, result.Count);
            PointAssert.Contains(result, new Point(100, 150));
            PointAssert.Contains(result, new Point(200, 200));
            PointAssert.Contains(result, new Point(0, 100));
            PointAssert.Contains(result, new Point(150, 50));
            PointAssert.Contains(result, new Point(50, 250));
        }

        [Fact]
        public void GetAttributes() {
            var circle = new Circle(new Point(100, 150), new Point(200, 250));

            var attributes = circle.GetAttributes().ToList();

            Assert.Equal(3, attributes.Count);
            Assert.Equal("100", Assert.Single(attributes, a => a.Key == "cx").Value);
            Assert.Equal("150", Assert.Single(attributes, a => a.Key == "cy").Value);
            Assert.Equal("141.4213562373095", Assert.Single(attributes, a => a.Key == "r").Value);
        }

        [Fact]
        public void Anchors_Get() {
            var circle = new Circle(new Point(100, 150), new Point(200, 250));

            var result = circle.Anchors;

            Assert.Equal(2, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0].Get(circle));
            PointAssert.Equal(new Point(200, 250), result[1].Get(circle));
        }

        [Fact]
        public void Anchors_Set() {
            var circle = new Circle(new Point(100, 150), new Point(200, 250));

            var result = circle.Anchors;

            Assert.Equal(2, result.Count);
            result[0].Set(circle, new Point(110, 160));
            result[1].Set(circle, new Point(210, 260));
            PointAssert.Equal(new Point(110, 160), circle.CenterPoint);
            PointAssert.Equal(new Point(210, 260), circle.RadiusPoint);
        }

        [Fact]
        public void Clone() {
            var circle = new Circle(new Point(100, 150), new Point(200, 250));

            var result = circle.Clone();

            var resultCircle = Assert.IsType<Circle>(result);

            Assert.NotSame(circle, result);
            PointAssert.Equal(new Point(100, 150), resultCircle.CenterPoint);
            PointAssert.Equal(new Point(200, 250), resultCircle.RadiusPoint);
        }
    }
}
