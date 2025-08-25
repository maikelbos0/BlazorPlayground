using BlazorPlayground.StateManagement;

namespace BlazorPlayground.Web.Client;

public class ApplicationState(IStateProvider stateProvider) {
    public MutableState<int> Counter { get; } = stateProvider.Mutable(0);
}
