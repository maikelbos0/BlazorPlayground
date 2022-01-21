using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class EvaluatableSymbolTests {
        [Fact]
        public void SymbolGroup_Can_Append_EvaluatableSymbol_When_Empty() {
            var number = new LiteralNumber(0);
            var symbols = new List<ISymbol>();

            Assert.True(number.TryAppendTo(symbols));
            Assert.Equal(number, Assert.Single(symbols));
        }

        [Fact]
        public void SymbolGroup_Can_Append_EvaluatableSymbol_When_Last_Symbol_Is_BinaryOperator() {
            var number = new LiteralNumber(0);
            var symbols = new List<ISymbol>() {
                new LiteralNumber(1),
                new AdditionOperator('+')
            };

            Assert.True(number.TryAppendTo(symbols));
            Assert.Equal(3, symbols.Count);
            Assert.Equal(number, symbols[2]);
        }

        [Fact]
        public void SymbolGroup_Can_Not_Append_EvaluatableSymbol_When_Last_Symbol_Is_EvaluatableSymbol() {
            var group = new SymbolGroup();
            var literalNumber = new LiteralNumber(1);

            group.Symbols.Add(new LiteralNumber(1));

            Assert.False(group.Append(literalNumber));
            Assert.Single(group.Symbols);
        }
    }
}
