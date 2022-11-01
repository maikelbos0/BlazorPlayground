using System;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class ETests {
        [Fact]
        public void E_Evaluate_Returns_E() {
            var e = new E();

            Assert.Equal((decimal)Math.E, e.Evaluate());
        }

        [Fact]
        public void E_ToString_Succeeds() {
            var e = new E();

            Assert.Equal("e", e.ToString());
        }
    }
}
