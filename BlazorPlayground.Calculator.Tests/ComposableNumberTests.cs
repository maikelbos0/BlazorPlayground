using System.Linq;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class ComposableNumberTests {
        [Theory]
        [InlineData("", '0')]
        [InlineData("1", '5')]
        [InlineData("1.", '5')]
        public void ComposableNumber_Can_Append_Digit(string previousCharacters, char digit) {
            var number = new ComposableNumber();

            number.Characters.AddRange(previousCharacters);

            Assert.True(number.Append(digit));
            Assert.Equal(previousCharacters.Length + 1, number.Characters.Count);
            Assert.Equal(digit, number.Characters.Last());
        }

        [Theory]
        [InlineData("", '.')]
        [InlineData("1", '.')]
        [InlineData("", ',')]
        [InlineData("1", ',')]
        public void ComposableNumber_Can_Append_DecimalSeparator(string previousCharacters, char separator) {
            var number = new ComposableNumber();

            number.Characters.AddRange(previousCharacters);

            Assert.True(number.Append(separator));
            Assert.Equal(previousCharacters.Length + 1, number.Characters.Count);
            Assert.Equal(separator, number.Characters.Last());
        }

        [Theory]
        [InlineData(".", '.')]
        [InlineData(",", '.')]
        [InlineData("1.", '.')]
        [InlineData("1,", '.')]
        [InlineData(".", ',')]
        [InlineData(",", ',')]
        [InlineData("1.", ',')]
        [InlineData("1,", ',')]
        public void ComposableNumber_Can_Not_Append_DecimalSeparator_When_Present(string previousCharacters, char separator) {
            var number = new ComposableNumber();

            number.Characters.AddRange(previousCharacters);

            Assert.False(number.Append(separator));
            Assert.Equal(previousCharacters.Length, number.Characters.Count);
        }

        [Theory]
        [InlineData("", 'a')]
        [InlineData("1", 'a')]
        [InlineData("", ' ')]
        [InlineData("1", ' ')]
        public void ComposableNumber_Can_Not_Append_Other_Characters(string previousCharacters, char character) {
            var number = new ComposableNumber();

            number.Characters.AddRange(previousCharacters);

            Assert.False(number.Append(character));
            Assert.Equal(previousCharacters.Length, number.Characters.Count);
        }

        [Theory]
        [InlineData('-')]
        [InlineData('−')]
        [InlineData('±')]
        public void ComposableNumber_Append_Minus_Creates_Negative_Number(char minus) {
            var number = new ComposableNumber();

            number.Characters.Add('5');

            Assert.True(number.Append(minus));
            Assert.True(number.IsNegative);
        }

        [Theory]
        [InlineData('-', '-')]
        [InlineData('−', '-')]
        [InlineData('±', '-')]
        [InlineData('-', '−')]
        [InlineData('−', '−')]
        [InlineData('±', '−')]
        [InlineData('-', '±')]
        [InlineData('−', '±')]
        [InlineData('±', '±')]
        public void ComposableNumber_Append_Minus_Twice_Creates_Positive_Number(char firstMinus, char secondMinus) {
            var number = new ComposableNumber();

            number.Characters.Add('5');

            Assert.True(number.Append(firstMinus));
            Assert.True(number.Append(secondMinus));
            Assert.False(number.IsNegative);
        }

        [Theory]
        [InlineData(false, ".25", 0.25)]
        [InlineData(false, "25.25", 25.25)]
        [InlineData(false, "25", 25)]
        [InlineData(false, ",25", 0.25)]
        [InlineData(false, "25,25", 25.25)]
        [InlineData(true, ".25", -0.25)]
        [InlineData(true, "25.25", -25.25)]
        [InlineData(true, "25", -25)]
        [InlineData(true, ",25", -0.25)]
        [InlineData(true, "25,25", -25.25)]
        public void ComposableNumber_Evaluate_Succeeds(bool isNegative, string characters, decimal expectedValue) {
            var number = new ComposableNumber();

            number.IsNegative = isNegative;
            number.Characters.AddRange(characters);

            Assert.Equal(expectedValue, number.Evaluate());
        }

        [Theory]
        [InlineData(false, ".25", ".25")]
        [InlineData(false, "25.25", "25.25")]
        [InlineData(true, ".25", "-.25")]
        [InlineData(true, "25.25", "-25.25")]
        public void ComposableNumber_ToString_Succeeds(bool isNegative, string characters, string expectedValue) {
            var number = new ComposableNumber();

            number.IsNegative = isNegative;
            number.Characters.AddRange(characters);

            Assert.Equal(expectedValue, number.ToString());
        }
    }
}
