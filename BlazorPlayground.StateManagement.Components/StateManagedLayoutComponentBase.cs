using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace BlazorPlayground.StateManagement.Components;

public abstract class StateManagedLayoutComponentBase : StateManagedComponentBase {
    /// <summary>
    /// Gets the content to be rendered inside the layout.
    /// </summary>
    [Parameter]
    public RenderFragment? Body { get; set; }

    /// <inheritdoc />
    // Derived instances of StateManagedLayoutComponentBase do not appear in any statically analyzable
    // calls of OpenComponent<T> where T is well-known. Consequently we have to explicitly provide a hint to the trimmer to preserve
    // properties.
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(LayoutComponentBase))]
    public override async Task SetParametersAsync(ParameterView parameters) {
        await base.SetParametersAsync(parameters);
        Evaluate();
    }
}
