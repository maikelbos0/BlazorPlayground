using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.Calculator.Tests;

public class BackspaceTests {
    [Fact]
    public void Backspace_TryAppendTo_Succeeds_When_Last_Symbol_Is_UnaryOperator_With_Symbol() {
        var backspace = new Backspace();
        var symbols = new List<ISymbol>() {
            new NegationOperator() {
                Symbol = new LiteralNumber(4)
            }
        };

        Assert.True(backspace.TryAppendTo(symbols));
        Assert.IsType<LiteralNumber>(Assert.Single(symbols));
    }

    [Fact]
    public void Backspace_TryAppendTo_Succeeds_When_Last_Symbol_Is_UnaryOperator_Without_Symbol() {
        var backspace = new Backspace();
        var symbols = new List<ISymbol>() {
            new NegationOperator()
        };

        Assert.True(backspace.TryAppendTo(symbols));
        Assert.Empty(symbols);
    }

    [Fact]
    public void Backspace_TryAppendTo_Succeeds_When_Last_Symbol_Is_BinaryOperator() {
        var backspace = new Backspace();
        var symbols = new List<ISymbol>() {
            new LiteralNumber(4),
            new AdditionOperator('+')
        };

        Assert.True(backspace.TryAppendTo(symbols));
        Assert.IsType<LiteralNumber>(Assert.Single(symbols));
    }

    [Fact]
    public void Backspace_TryAppendTo_Succeeds_When_Last_Symbol_Is_ComposableNumber() {
        var backspace = new Backspace();
        var number = new ComposableNumber();
        var symbols = new List<ISymbol>() {
            number
        };

        number.Characters.AddRange('4', '5');

        Assert.True(backspace.TryAppendTo(symbols));
        Assert.Single(symbols);
        Assert.Single(number.Characters);
    }

    [Theory]
    [InlineData]
    [InlineData('4')]
    public void Backspace_TryAppendTo_Succeeds_When_Last_ComposableNumber_Has_Less_Than_Two_Characters(params char[] characters) {
        var backspace = new Backspace();
        var number = new ComposableNumber();
        var symbols = new List<ISymbol>() {
            number
        };

        number.Characters.AddRange(characters);

        Assert.True(backspace.TryAppendTo(symbols));
        Assert.Empty(symbols);
    }

    [Fact]
    public void Backspace_TryAppendTo_Succeeds_For_LiteralNumber() {
        var backspace = new Backspace();
        var number = new LiteralNumber(4);
        var symbols = new List<ISymbol>() {
            number
        };

        Assert.True(backspace.TryAppendTo(symbols));
        Assert.Empty(symbols);
    }

    [Fact]
    public void Backspace_TryAppendTo_Fails_When_Symbols_Is_Empty() {
        var backspace = new Backspace();
        var symbols = new List<ISymbol>();

        Assert.False(backspace.TryAppendTo(symbols));
        Assert.Empty(symbols);
    }
}
