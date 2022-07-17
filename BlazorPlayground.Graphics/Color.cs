using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorPlayground.Graphics {
    public class Color {
        private class Parser {
            public Parser(Regex matcher, Func<string, Match, Color> constructor) {
                Matcher = matcher;
                Constructor = constructor;
            }

            public Regex Matcher { get; }
            public Func<string, Match, Color> Constructor { get; }
        }

        private readonly static Parser[] parsers = new[] {
            new Parser(
                new Regex("^#(?<Red>[0-9a-fA-Z]{2})(?<Green>[0-9a-fA-Z]{2})(?<Blue>[0-9a-fA-Z]{2})$"),
                (value, match) => new Color(value, byte.Parse(match.Groups["Red"].Value, NumberStyles.HexNumber), byte.Parse(match.Groups["Green"].Value, NumberStyles.HexNumber), byte.Parse(match.Groups["Blue"].Value, NumberStyles.HexNumber), 1)
            )
        };

        public string ColorValue { get; }
        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public double Alpha { get; }

        public Color(string colorValue, byte red, byte green, byte blue, double alpha) {
            ColorValue = colorValue;
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public override string ToString() => ColorValue;

        public static implicit operator Color(string colorValue) {
            foreach (var parser in parsers) {
                var match = parser.Matcher.Match(colorValue);

                if (match.Success) {
                    return parser.Constructor(colorValue, match);
                }
            }

            var color = System.Drawing.Color.FromName(colorValue);

            return new Color(colorValue, color.R, color.G, color.B, 1);
        }
    }
}
