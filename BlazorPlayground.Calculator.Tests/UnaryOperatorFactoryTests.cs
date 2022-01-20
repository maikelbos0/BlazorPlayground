using System;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class UnaryOperatorFactoryTests {
        [Theory]
        [InlineData('±', typeof(NegationOperator))]
        public void UnaryOperatorFactory_Creates_Correct_Operator(char character, Type expectedType) {
            var op = UnaryOperatorFactory.GetOperator(character, new LiteralNumber(5.5M));

            Assert.IsType(expectedType, op);
        }

        [Theory]
        [InlineData('0')]
        [InlineData('9')]
        [InlineData('.')]
        [InlineData(',')]
        [InlineData('/')]
        [InlineData('÷')]
        [InlineData('*')]
        [InlineData('×')]
        [InlineData('-')]
        [InlineData('−')]
        [InlineData('+')]
        public void UnaryOperatorFactory_Does_Not_Create_Operator_For_Unknown_Operator(char character) {
            var op = UnaryOperatorFactory.GetOperator(character, new LiteralNumber(5.5M));

            Assert.Null(op);
        }
    }
}
