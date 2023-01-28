namespace BlazorPlayground.Chart;

public class DataSeries : List<double?> {
    public string Name { get; set; }

    public DataSeries(string name) {
        Name = name;
    }
}
