using NSubstitute;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class DependencyBaseTests {
    private class Dependency : DependencyBase {
        public new void EvaluateDependents() {
            base.EvaluateDependents();
        }
    }

    [Fact]
    public void AddDependent() {
        var dependent = Substitute.For<IDependent>();
        var subject = new Dependency();

        subject.AddDependent(dependent);
        subject.AddDependent(dependent);

        subject.EvaluateDependents();

        dependent.Received(1).Evaluate();
    }
}
