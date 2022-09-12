using System;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class PointAssert {
        public static void Equal(Point expected, Point? actual) {
            Assert.NotNull(actual);
            Assert.Equal(expected.X, actual!.X, 1);
            Assert.Equal(expected.Y, actual!.Y, 1);
        }

        public static void Contains(IEnumerable<Point> collection, Point actual) {
            Assert.Contains(collection, p => Math.Abs(p.X - actual.X) < 0.1 && Math.Abs(p.Y - actual.Y) < 0.1);
        }

        public static void DoesNotContain(IEnumerable<Point> collection, Point actual) {
            Assert.DoesNotContain(collection, p => Math.Abs(p.X - actual.X) < 0.1 && Math.Abs(p.Y - actual.Y) < 0.1);
        }
    }
}
