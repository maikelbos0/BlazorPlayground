using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorPlayground.Components {
    public partial class Wizard : ComponentBase {

        [Parameter] public string ContainerClass { get; set; }
        [Parameter] public string TitleClass { get; set; }
        [Parameter] public RenderFragment TitleContent { get; set; }
        [Parameter] public string MenuClass { get; set; }
        [Parameter] public string MenuItemClass { get; set; }
        [Parameter] public string ActiveMenuItemClass { get; set; }
        [Parameter] public string ButtonBarClass { get; set; }
        [Parameter] public string ButtonClass { get; set; }
        [Parameter] public string ButtonNextText { get; set; } = "Next";
        [Parameter] public string ButtonFinishText { get; set; } = "Finish";
        [Parameter] public string ContentClass { get; set; }
        [Parameter] public RenderFragment Steps { get; set; }
        [Parameter] public IList<WizardComponent> LayoutOrder { get; set; } = new List<WizardComponent>();

        private List<WizardStep> StepsInternal { get; set; } = new List<WizardStep>();
        private int? ActiveStepIndex { get; set; }
        internal WizardStep ActiveStep => ActiveStepIndex.HasValue && StepsInternal.Count > ActiveStepIndex.Value ? StepsInternal[ActiveStepIndex.Value] : null;
        public bool IsActive => ActiveStepIndex.HasValue;

        public void Start() {
            if (IsActive) {
                return;
            }

            ActiveStepIndex = 0;
            StateHasChanged();
        }

        public void Cancel() {
            if (!IsActive) {
                return;
            }

            ActiveStepIndex = null;
            StepsInternal.Clear();
        }

        // TODO cancel event
        // TODO finish event

        internal async Task AddStep(WizardStep step) {
            if (!StepsInternal.Contains(step)) {
                StepsInternal.Add(step);

                if (StepsInternal.Count == 1) {
                    await ActiveStep.InitializeStep(new InitializeStepEventArgs());
                }
            }
        }

        private async Task TryCompleteStep() {
            var args = await ActiveStep.TryCompleteStep(new TryCompleteStepEventArgs());

            if (!args.IsCancelled) {

                ActiveStepIndex++;

                if (ActiveStep == null) {
                    ActiveStepIndex = null;
                    StepsInternal.Clear();
                }
                else {
                    await ActiveStep.InitializeStep(new InitializeStepEventArgs());
                }
            }
        }
    }
}
