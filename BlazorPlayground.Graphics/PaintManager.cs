using System.Globalization;
using System.Text.RegularExpressions;

namespace BlazorPlayground.Graphics {
    public class PaintManager {
        private class ColorParser {
            public ColorParser(Regex matcher, Func<Match, Color> constructor) {
                Matcher = matcher;
                Constructor = constructor;
            }

            public Regex Matcher { get; }
            public Func<Match, Color> Constructor { get; }
        }

        private readonly static ColorParser[] colorParsers = new[] {
            new ColorParser(
                new Regex(@"^#(?<Red>[0-9a-f]{2})(?<Green>[0-9a-f]{2})(?<Blue>[0-9a-f]{2})$", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                match => new Color(
                    byte.Parse(match.Groups["Red"].Value, NumberStyles.HexNumber),
                    byte.Parse(match.Groups["Green"].Value, NumberStyles.HexNumber),
                    byte.Parse(match.Groups["Blue"].Value, NumberStyles.HexNumber),
                    1
                )
            ),
            new ColorParser(
                new Regex(@"^#(?<Red>[0-9a-f])(?<Green>[0-9a-f])(?<Blue>[0-9a-f])$", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                match => new Color(
                    (byte)(byte.Parse(match.Groups["Red"].Value, NumberStyles.HexNumber) * 17),
                    (byte)(byte.Parse(match.Groups["Green"].Value, NumberStyles.HexNumber) * 17),
                    (byte)(byte.Parse(match.Groups["Blue"].Value, NumberStyles.HexNumber) * 17),
                    1
                )
            ),
            new ColorParser(
                new Regex(@"^rgb\s*\(\s*(?<Red>[01]?\d{1,2}|2[0-4]\d|25[0-5])\s*,\s*(?<Green>[01]?\d{1,2}|2[0-4]\d|25[0-5])\s*,\s*(?<Blue>[01]?\d{1,2}|2[0-4]\d|25[0-5])\s*\)$", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                match => new Color(
                    byte.Parse(match.Groups["Red"].Value, CultureInfo.InvariantCulture),
                    byte.Parse(match.Groups["Green"].Value, CultureInfo.InvariantCulture),
                    byte.Parse(match.Groups["Blue"].Value, CultureInfo.InvariantCulture),
                    1
                )
            ),
            new ColorParser(
                new Regex(@"^rgba\s*\(\s*(?<Red>[01]?\d{1,2}|2[0-4]\d|25[0-5])\s*,\s*(?<Green>[01]?\d{1,2}|2[0-4]\d|25[0-5])\s*,\s*(?<Blue>[01]?\d{1,2}|2[0-4]\d|25[0-5])\s*,\s*(?<Alpha>0|1(\.0+)?|0?\.\d+)\s*\)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                match => new Color(
                    byte.Parse(match.Groups["Red"].Value, CultureInfo.InvariantCulture),
                    byte.Parse(match.Groups["Green"].Value, CultureInfo.InvariantCulture),
                    byte.Parse(match.Groups["Blue"].Value, CultureInfo.InvariantCulture),
                    double.Parse(match.Groups["Alpha"].Value, CultureInfo.InvariantCulture)
                )
            )
        };
        private readonly static Color defaultColor = new(0, 0, 0, 1);

        internal static Color ParseColor(string? colorValue) {
            if (colorValue == null) {
                return defaultColor;
            }

            foreach (var parser in colorParsers) {
                var match = parser.Matcher.Match(colorValue.Trim());

                if (match.Success) {
                    return parser.Constructor(match);
                }
            }

            var color = System.Drawing.Color.FromName(colorValue.Trim());

            return new Color(color.R, color.G, color.B, 1);
        }

        private string? colorValue;

        public PaintMode Mode { get; set; } = PaintMode.None;
        public string? ColorValue {
            get => colorValue;
            set {
                colorValue = value;
                Color = ParseColor(colorValue);
            }
        }
        public Color Color { get; private set; } = defaultColor;
        public IPaintServer Server => Mode switch {
            PaintMode.Color => Color,
            PaintMode.None => PaintServer.None,
            _ => throw new NotImplementedException($"No implementation found for {nameof(PaintMode)} '{Mode}'.")
        };
    }
}
