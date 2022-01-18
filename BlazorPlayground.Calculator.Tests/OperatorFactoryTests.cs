using System;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class OperatorFactoryTests {
        [Theory]
        [InlineData('/', typeof(DivisionOperator))]
        [InlineData('÷', typeof(DivisionOperator))]
        [InlineData('*', typeof(MultiplicationOperator))]
        [InlineData('×', typeof(MultiplicationOperator))]
        [InlineData('-', typeof(SubtractionOperator))]
        [InlineData('−', typeof(SubtractionOperator))]
        [InlineData('+', typeof(AdditionOperator))]
        public void OperatorFactory_Creates_Correct_Operator(char character, Type expectedType) {
            var op = OperatorFactory.GetOperator(character);

            Assert.IsType(expectedType, op);
        }

        [Theory]
        [InlineData('0')]
        [InlineData('9')]
        [InlineData('.')]
        [InlineData(',')]
        [InlineData('±')]
        public void OperatorFactory_Does_Not_Create_Operator_For_Numeric_Value(char character) {
            var op = OperatorFactory.GetOperator(character);

            Assert.Null(op);
        }
    }
}
