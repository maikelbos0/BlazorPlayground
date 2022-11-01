using System;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class SymbolFactoryTests {
        [Theory]
        [InlineData('/', typeof(DivisionOperator))]
        [InlineData('÷', typeof(DivisionOperator))]
        [InlineData('*', typeof(MultiplicationOperator))]
        [InlineData('×', typeof(MultiplicationOperator))]
        [InlineData('-', typeof(SubtractionOperator))]
        [InlineData('−', typeof(SubtractionOperator))]
        [InlineData('+', typeof(AdditionOperator))]
        [InlineData('±', typeof(NegationOperator))]
        [InlineData('⁻', typeof(ReciprocalOperator))]
        [InlineData('|', typeof(AbsoluteOperator))]
        [InlineData('%', typeof(ModulusOperator))]
        [InlineData('²', typeof(SquareOperator))]
        [InlineData('√', typeof(SquareRootOperator))]
        [InlineData('π', typeof(Pi))]
        [InlineData('e', typeof(E))]
        [InlineData('⌫', typeof(Backspace))]
        public void SymbolFactory_Creates_Correct_Operator(char character, Type expectedType) {
            var symbol = SymbolFactory.GetSymbol(character);

            Assert.IsType(expectedType, symbol);
        }

        [Theory]
        [InlineData('0')]
        [InlineData('9')]
        [InlineData('.')]
        [InlineData(',')]
        [InlineData('a')]
        [InlineData('z')]
        public void SymbolFactory_Creates_Character_For_Unknown_Operator(char character) {
            var symbol = SymbolFactory.GetSymbol(character);

            Assert.IsType<Character>(symbol);
        }
    }
}
