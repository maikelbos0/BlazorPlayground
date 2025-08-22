using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateManagementServiceCollectionExtensionsTests {
    [Fact]
    public void AddStateProvider() {
        var subject = new ServiceCollection();

        subject.AddStateProvider2();

        var result = Assert.Single(subject);
        Assert.Equal(ServiceLifetime.Singleton, result.Lifetime);
        Assert.Equal(typeof(IStateProvider), result.ServiceType);
        Assert.Equal(typeof(StateProvider), result.ImplementationType);
    }
}
