using NSubstitute;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class DependencyBaseTests {
    private class Dependency : DependencyBase {
        public ICollection<IDependent> GetDependents()
            => dependents.Keys;
    }

    [Fact]
    public void AddDependent() {
        var dependent = Substitute.For<IDependent>();
        var subject = new Dependency();

        subject.AddDependent(dependent);
        subject.AddDependent(dependent);

        Assert.Equal([dependent], subject.GetDependents());
    }
}
