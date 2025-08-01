﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class GameElementProviderTests {
    [Fact]
    public async Task CreateShip() {
        const string baseUrl = "https://localhost";
        const string assetContent = @"
        {
            ""Sections"": [
                {
                    ""Geometry"": {
                        ""type"": ""Polygon"",
                        ""coordinates"": [
                            [
                                [ 0, 40 ],
                                [ -50, 0 ],
                                [ -40, -30 ],
                                [ -20, 0 ],
                                [ 0, 10 ],
                                [ 0, 40 ]
                            ]
                        ]
                    },
                    ""FillColor"": ""rgba(0, 0, 0, 0)"",
                    ""StrokeColor"": ""#00FFFF"",
                    ""StrokeWidth"": 1,
                    ""Opacity"": 0.5
                }
            ]
        }";

        var httpMessageHandler = new MockHttpMessageHandler(assetContent);
        var httpClient = new HttpClient(httpMessageHandler) { BaseAddress = new(baseUrl) };
        var subject = new GameElementProvider(httpClient);

        var result = await subject.CreateShip(new(100, 200));

        Assert.NotNull(httpMessageHandler.ReceivedRequest);
        Assert.Equal(HttpMethod.Get, httpMessageHandler.ReceivedRequest.Method);
        Assert.Equal(new Uri($"{baseUrl}/{GameElementProvider.AssetLocation}/{GameElementProvider.ShipAssetName}.json"), httpMessageHandler.ReceivedRequest.RequestUri);

        Assert.NotNull(result);
        Assert.Equal(new(100, 200), result.Position);

        var sectionResult = Assert.Single(result.Sections);
        Assert.Equal(6, sectionResult.Coordinates.Length);
        Assert.Equal("rgba(0, 0, 0, 0)", sectionResult.FillColor);
        Assert.Equal("#00FFFF", sectionResult.StrokeColor);
        Assert.Equal(1, sectionResult.StrokeWidth);
        Assert.Equal(0.5, sectionResult.Opacity);
    }
}
