﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class DataSeries2 : ComponentBase {
    public const string FallbackColor = "#000000";

    public static List<string> DefaultColors { get; set; } = new() {
        // https://coolors.co/550527-688e26-faa613-f44708-a10702
        // "#550527", "#688e26", "#faa613", "#f44708", "#a10702"

        // https://coolors.co/264653-2a9d8f-e9c46a-f4a261-e76f51
        // "#264653", "#2a9d8f", "#e9c46a", "#f4a261", "#e76f51"
        
        // https://coolors.co/2274a5-f75c03-f1c40f-d90368-00cc66
        "#2274a5", "#f75c03", "#f1c40f", "#d90368", "#00cc66"
    };

    [CascadingParameter] internal LayerBase2 Layer { get; set; } = null!;
    [Parameter] public string? Name { get; set; }
    [Parameter] public string? Color { get; set; }
    [Parameter] public List<decimal?> DataPoints { get; set; } = new();

    protected override void OnInitialized() {
        if (!Layer.DataSeries.Contains(this)) {
            Layer.DataSeries.Add(this);
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(1, nameof(DataSeries2));

    public string GetColor() {
        if (Color != null) {
            return Color;
        }

        var index = Layer.DataSeries.IndexOf(this);

        if (DefaultColors.Any() && index >= 0) {
            return DefaultColors[index % DefaultColors.Count];
        }

        return FallbackColor;
    }
}
