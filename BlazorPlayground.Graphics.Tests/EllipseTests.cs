using System.Linq;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class EllipseTests {
        [Fact]
        public void ElementName() {
            var ellipse = new Ellipse(new Point(100, 150), new Point(200, 250));

            Assert.Equal("ellipse", ellipse.ElementName);
        }

        [Fact]
        public void GetAttributes() {
            var ellipse = new Ellipse(new Point(100, 150), new Point(50, 75));

            var attributes = ellipse.GetAttributes().ToList();

            Assert.Equal(4, attributes.Count);
            Assert.Equal("100", Assert.Single(attributes, a => a.Key == "cx").Value);
            Assert.Equal("150", Assert.Single(attributes, a => a.Key == "cy").Value);
            Assert.Equal("50", Assert.Single(attributes, a => a.Key == "rx").Value);
            Assert.Equal("75", Assert.Single(attributes, a => a.Key == "ry").Value);
        }

        [Fact]
        public void GetSnapPoints() {
            var ellipse = new Ellipse(new Point(100, 150), new Point(200, 200));

            var result = ellipse.GetSnapPoints();

            Assert.Equal(5, result.Count);
            PointAssert.Contains(result, new Point(100, 150));
            PointAssert.Contains(result, new Point(200, 150));
            PointAssert.Contains(result, new Point(0, 150));
            PointAssert.Contains(result, new Point(100, 200));
            PointAssert.Contains(result, new Point(100, 100));
        }

        [Fact]
        public void Anchors_Get() {
            var ellipse = new Ellipse(new Point(100, 150), new Point(200, 250));

            var result = ellipse.Anchors;
            
            Assert.Equal(2, result.Count);
            PointAssert.Equal(new Point(100, 150), result[0].Get(ellipse));
            PointAssert.Equal(new Point(200, 250), result[1].Get(ellipse));
        }

        [Fact]
        public void Anchors_Set() {
            var ellipse = new Ellipse(new Point(100, 150), new Point(200, 250));

            var result = ellipse.Anchors;

            Assert.Equal(2, result.Count);
            result[0].Set(ellipse, new Point(110, 160));
            result[1].Set(ellipse, new Point(210, 260));
            PointAssert.Equal(new Point(110, 160), ellipse.CenterPoint);
            PointAssert.Equal(new Point(210, 260), ellipse.RadiusPoint);
        }

        [Fact]
        public void Clone() {
            var ellipse = new Ellipse(new Point(100, 150), new Point(200, 250));

            var result = ellipse.Clone();

            var resultEllipse = Assert.IsType<Ellipse>(result);

            Assert.NotSame(ellipse, resultEllipse);
            PointAssert.Equal(new Point(100, 150), resultEllipse.CenterPoint);
            PointAssert.Equal(new Point(200, 250), resultEllipse.RadiusPoint);
        }
    }
}
