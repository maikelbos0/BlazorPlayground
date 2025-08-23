using System;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class WeakReferenceEqualityComparerTests {
    private class Target {
        public int HashCode { get; }

        public Target(int hashCode) {
            HashCode = hashCode;
        }

        public override int GetHashCode() => HashCode;
    }

    [Fact]
    public void Equals_Null_WeakReferences() {
        var subject = WeakReferenceEqualityComparer<Target>.Instance;

        Assert.True(subject.Equals(null, null));
    }

    [Fact]
    public void Equals_Same_WeakReference() {
        var subject = WeakReferenceEqualityComparer<Target>.Instance;
        var weakReference = new WeakReference<Target>(new Target(42));

        Assert.True(subject.Equals(weakReference, weakReference));
    }

    [Fact]
    public void Equals_Same_Target() {
        var subject = WeakReferenceEqualityComparer<Target>.Instance;
        var target = new Target(42);

        Assert.True(subject.Equals(new WeakReference<Target>(target), new WeakReference<Target>(target)));
    }

    [Fact]
    public void Equals_Null_Targets() {
        var subject = WeakReferenceEqualityComparer<Target>.Instance;

        Assert.True(subject.Equals(new WeakReference<Target>(null!), new WeakReference<Target>(null!)));
    }

    [Fact]
    public void Equals_Different_Target() {
        var subject = WeakReferenceEqualityComparer<Target>.Instance;
        var weakReferenceX = new WeakReference<Target>(new Target(42));
        var weakReferenceY = new WeakReference<Target>(new Target(42));

        Assert.False(subject.Equals(weakReferenceX, weakReferenceY));
    }

    [Fact]
    public void GetHashCode_Returns_Target_HashCode() {
        var subject = WeakReferenceEqualityComparer<Target>.Instance;
        var weakReference = new WeakReference<Target>(new Target(42));

        Assert.Equal(42, subject.GetHashCode(weakReference));
    }

    [Fact]
    public void GetHashCode_Returns_Zero_For_Null_Target() {
        var subject = WeakReferenceEqualityComparer<Target>.Instance;
        var weakReference = new WeakReference<Target>(null!);

        Assert.Equal(0, subject.GetHashCode(weakReference));
    }
}
