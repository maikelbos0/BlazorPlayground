using BlazorPlayground.StateManagement;
using Microsoft.Extensions.DependencyInjection.Extensions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class StateManagementServiceCollectionExtensions {
    public static IServiceCollection AddStateProvider2(this IServiceCollection services) {
        services.TryAddSingleton<IStateProvider, StateProvider>();
        return services;
    }
}
