using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorPlayground.Components {
    public partial class WizardStep : ComponentBase {
        [CascadingParameter]
        private Wizard Parent { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public EventCallback<InitializeStepEventArgs> OnInitializeStep { get; set; }

        [Parameter]
        public EventCallback<TryCompleteStepEventArgs> OnTryCompleteStep { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public bool IsActive => Parent.ActiveStep == this;

        protected override async Task OnInitializedAsync() {
            await Parent.AddStep(this);
        }

        internal virtual async Task<InitializeStepEventArgs> InitializeStep(InitializeStepEventArgs args) {
            await OnInitializeStep.InvokeAsync(args);

            return args;
        }

        internal virtual async Task<TryCompleteStepEventArgs> TryCompleteStep(TryCompleteStepEventArgs args) {
            await OnTryCompleteStep.InvokeAsync(args);

            return args;
        }
    }
}
