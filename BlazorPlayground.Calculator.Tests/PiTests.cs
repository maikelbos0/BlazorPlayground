using System;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class PiTests {
        [Fact]
        public void Pi_Evaluate_Returns_Pi() {
            var pi = new Pi();

            Assert.Equal((decimal)Math.PI, pi.Evaluate());
        }

        [Fact]
        public void Pi_ToString_Succeeds() {
            var pi = new Pi();

            Assert.Equal("π", pi.ToString());
        }
    }
}
