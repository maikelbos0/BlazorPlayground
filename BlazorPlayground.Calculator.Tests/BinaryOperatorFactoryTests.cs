using System;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class BinaryOperatorFactoryTests {
        [Theory]
        [InlineData('/', typeof(DivisionOperator))]
        [InlineData('÷', typeof(DivisionOperator))]
        [InlineData('*', typeof(MultiplicationOperator))]
        [InlineData('×', typeof(MultiplicationOperator))]
        [InlineData('-', typeof(SubtractionOperator))]
        [InlineData('−', typeof(SubtractionOperator))]
        [InlineData('+', typeof(AdditionOperator))]
        public void BinaryOperatorFactory_Creates_Correct_Operator(char character, Type expectedType) {
            var op = BinaryOperatorFactory.GetOperator(character);

            Assert.IsType(expectedType, op);
        }

        [Theory]
        [InlineData('0')]
        [InlineData('9')]
        [InlineData('.')]
        [InlineData(',')]
        [InlineData('±')]
        public void BinaryOperatorFactory_Does_Not_Create_Operator_For_Unknown_Operator(char character) {
            var op = BinaryOperatorFactory.GetOperator(character);

            Assert.Null(op);
        }
    }
}
