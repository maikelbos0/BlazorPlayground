using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class BackspaceTests {
        [Fact]
        public void Backspace_TryAppendTo_Succeeds_When_Last_Symbol_Is_Unary_Operator_For_ComposableNumber() {
            var backspace = new Backspace();
            var number = new ComposableNumber();
            var symbols = new List<ISymbol>() {
                new NegationOperator() {
                    Symbol = new NegationOperator(){
                        Symbol = number
                    }
                }                
            };

            number.Characters.Add('4');

            Assert.True(backspace.TryAppendTo(symbols));
            Assert.Single(symbols);
            Assert.Empty(number.Characters);
        }

        [Fact]
        public void Backspace_TryAppendTo_Succeeds_When_Last_Symbol_Is_ComposableNumber() {
            var backspace = new Backspace();
            var number = new ComposableNumber();
            var symbols = new List<ISymbol>() {
                number
            };

            number.Characters.Add('4');

            Assert.True(backspace.TryAppendTo(symbols));
            Assert.Empty(number.Characters);
        }

        [Fact]
        public void Backspace_TryAppendTo_Fails_When_Symbols_Is_Empty() {
            var backspace = new Backspace();
            var symbols = new List<ISymbol>();

            Assert.False(backspace.TryAppendTo(symbols));
            Assert.Empty(symbols);
        }

        [Fact]
        public void Backspace_TryAppendTo_Fails_When_Last_ComposableNumber_Is_Empty() {
            var backspace = new Backspace();
            var number = new ComposableNumber();
            var symbols = new List<ISymbol>() {
                number
            };

            Assert.False(backspace.TryAppendTo(symbols));
            Assert.Empty(number.Characters);
        }
    }
}
