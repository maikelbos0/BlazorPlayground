using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class CharacterTests {
        [Fact]
        public void Character_TryAppendTo_Succeeds_When_Symbols_Is_Empty() {
            var character = new Character('1');
            var symbols = new List<ISymbol>();

            Assert.True(character.TryAppendTo(symbols));
            Assert.Equal('1', Assert.Single(Assert.IsType<ComposableNumber>(Assert.Single(symbols)).Characters));
        }

        [Fact]
        public void Character_TryAppendTo_Succeeds_When_Last_Symbol_Is_Unary_Operator_For_ComposableNumber() {
            var character = new Character('1');
            var number = new ComposableNumber();
            var symbols = new List<ISymbol>() {
                new NegationOperator() {
                    Symbol = new NegationOperator(){
                        Symbol = number
                    }
                }
            };

            Assert.True(character.TryAppendTo(symbols));
            Assert.Single(symbols);
            Assert.Equal('1', Assert.Single(number.Characters));
        }

        [Fact]
        public void Character_TryAppendTo_Succeeds_When_Last_Symbol_Is_ComposableNumber() {
            var character = new Character('1');
            var symbols = new List<ISymbol>() {
                new ComposableNumber()
            };

            Assert.True(character.TryAppendTo(symbols));
            Assert.Equal('1', Assert.Single(Assert.IsType<ComposableNumber>(Assert.Single(symbols)).Characters));
        }

        [Fact]
        public void Character_TryAppendTo_Succeeds_When_Last_Symbol_Is_BinaryOperator() {
            var character = new Character('1');
            var symbols = new List<ISymbol>() {
                new LiteralNumber(1),
                new AdditionOperator('+')
            };

            Assert.True(character.TryAppendTo(symbols));
            Assert.Equal(3, symbols.Count);
            Assert.Equal('1', Assert.Single(Assert.IsType<ComposableNumber>(symbols.Last()).Characters));
        }

        [Fact]
        public void Character_TryAppendTo_Fails_When_Last_Symbol_Is_Incompatible() {
            var character = new Character('1');
            var symbols = new List<ISymbol>() {
                new LiteralNumber(1)
            };

            Assert.False(character.TryAppendTo(symbols));
            Assert.Single(symbols);
        }

        [Fact]
        public void Character_TryAppendTo_Fails_For_Invalid_Character() {
            var character = new Character('a');
            var symbols = new List<ISymbol>();

            Assert.False(character.TryAppendTo(symbols));
            Assert.Empty(symbols);
        }
    }
}
