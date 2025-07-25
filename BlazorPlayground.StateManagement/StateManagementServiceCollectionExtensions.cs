using BlazorPlayground.StateManagement;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class StateManagementServiceCollectionExtensions {
    public static IServiceCollection AddStateProvider(this IServiceCollection services) {
        services.TryAddSingleton<IStateProvider, StateProvider>();
        return services;
    }
}
