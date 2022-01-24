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

            Assert.True(number.TryAppend(digit));
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

            Assert.True(number.TryAppend(separator));
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

            Assert.False(number.TryAppend(separator));
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

            Assert.False(number.TryAppend(character));
            Assert.Equal(previousCharacters.Length, number.Characters.Count);
        }

        [Fact]
        public void ComposableNumber_Can_Remove_Character() {
            var number = new ComposableNumber();

            number.Characters.Add('4');
            number.Characters.Add('5');

            Assert.True(number.TryRemoveCharacter());
            Assert.Equal('4', Assert.Single(number.Characters));
        }

        [Fact]
        public void ComposableNumber_TryRemoveCharacter_Does_Nothing_When_Emtpy() {
            var number = new ComposableNumber();

            Assert.False(number.TryRemoveCharacter());
            Assert.Empty(number.Characters);
        }

        [Theory]
        [InlineData(".25", 0.25)]
        [InlineData("25.25", 25.25)]
        [InlineData("25", 25)]
        [InlineData(",25", 0.25)]
        [InlineData("25,25", 25.25)]
        [InlineData("", 0)]
        public void ComposableNumber_Evaluate_Succeeds(string characters, decimal expectedValue) {
            var number = new ComposableNumber();

            number.Characters.AddRange(characters);

            Assert.Equal(expectedValue, number.Evaluate());
        }

        [Theory]
        [InlineData(".25", ".25")]
        [InlineData("25.25", "25.25")]
        [InlineData("", "0")]
        public void ComposableNumber_ToString_Succeeds(string characters, string expectedValue) {
            var number = new ComposableNumber();

            number.Characters.AddRange(characters);

            Assert.Equal(expectedValue, number.ToString());
        }
    }
}
