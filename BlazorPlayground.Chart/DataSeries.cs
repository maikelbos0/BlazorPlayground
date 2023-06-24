namespace BlazorPlayground.Chart;

public class DataSeries : List<double?> {
    public const string FallbackColor = "#000000";

    public static List<string> DefaultColors { get; set; } = new() {
        // https://coolors.co/550527-688e26-faa613-f44708-a10702
        "#550527", "#688e26", "#faa613", "#f44708", "#a10702"

        // https://coolors.co/264653-2a9d8f-e9c46a-f4a261-e76f51
        // "#264653", "#2a9d8f", "#e9c46a", "#f4a261", "#e76f51"
        
        // https://coolors.co/2274a5-f75c03-f1c40f-d90368-00cc66
        // "#2274a5", "#f75c03", "#f1c40f", "#d90368", "#00cc66"
    };

    public static string GetColor(int index) {
        if (DefaultColors.Any() && index >= 0) {
            return DefaultColors[index % DefaultColors.Count];
        }

        return FallbackColor;
    }

    public string Name { get; set; }

    public DataSeries(string name) {
        Name = name;
    }
}
