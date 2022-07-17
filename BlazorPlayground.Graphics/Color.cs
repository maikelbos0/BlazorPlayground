﻿using System.Globalization;
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
                new Regex("^#(?<Red>[0-9a-f]{2})(?<Green>[0-9a-f]{2})(?<Blue>[0-9a-f]{2})$", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                (value, match) => new Color(value, byte.Parse(match.Groups["Red"].Value, NumberStyles.HexNumber), byte.Parse(match.Groups["Green"].Value, NumberStyles.HexNumber), byte.Parse(match.Groups["Blue"].Value, NumberStyles.HexNumber), 1)
            ),
            new Parser(
                new Regex("^#(?<Red>[0-9a-f])(?<Green>[0-9a-f])(?<Blue>[0-9a-f])$", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                (value, match) => new Color(value, (byte)(byte.Parse(match.Groups["Red"].Value, NumberStyles.HexNumber) * 17), (byte)(byte.Parse(match.Groups["Green"].Value, NumberStyles.HexNumber) * 17), (byte)(byte.Parse(match.Groups["Blue"].Value, NumberStyles.HexNumber) * 17), 1)
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
                var match = parser.Matcher.Match(colorValue.Trim());

                if (match.Success) {
                    return parser.Constructor(colorValue, match);
                }
            }

            var color = System.Drawing.Color.FromName(colorValue.Trim());

            return new Color(colorValue, color.R, color.G, color.B, 1);
        }
    }
}
