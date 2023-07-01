using System.Collections;
using System.Globalization;

namespace BlazorPlayground.Chart.Shapes;

public class ShapeAttributeCollection : IEnumerable<KeyValuePair<string, object>> {
    private readonly Dictionary<string, object> attributes = new();

    public void Add(string key, decimal value) => attributes.Add(key, DecimalMath.Trim(value).ToString(CultureInfo.InvariantCulture));

    public void Add(string key, string value) => attributes.Add(key, value);

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => attributes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
