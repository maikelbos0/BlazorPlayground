using System;

namespace BlazorPlayground.Components {
    public class TryCompleteStepEventArgs : EventArgs {
        public bool IsCancelled { get; set; }
    }
}
