using System.Net.Http;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorPlayground.BulletHellBeastMode.Tests;

public class MockHttpMessageHandler : HttpMessageHandler {
    private readonly string returnValue;

    public HttpRequestMessage? ReceivedRequest { get; set; } = new();

    public MockHttpMessageHandler(string returnValue) {
        this.returnValue = returnValue;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        ReceivedRequest = request;

        return Task.FromResult(new HttpResponseMessage {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(returnValue)
        });
    }
}