using NSubstitute;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class DependencyGraphBuilderTests {
    [Fact]
    public void Constructor_Tracks_First_Dependent() {
        var trackedDependents = new ConcurrentDictionary<int, HashSet<IDependent>>();
        var dependent = Substitute.For<IDependent>();

        using var subject = new DependencyGraphBuilder(trackedDependents, dependent);

        var result = Assert.Single(trackedDependents);

        Assert.Equal(Environment.CurrentManagedThreadId, result.Key);
        Assert.Equivalent(new HashSet<IDependent>() { dependent }, result.Value);
    }
    [Fact]
    public void Constructor_Tracks_Second_Dependent() {
        var trackedDependents = new ConcurrentDictionary<int, HashSet<IDependent>>();
        var existingDependent = Substitute.For<IDependent>();
        var dependent = Substitute.For<IDependent>();

        Assert.True(trackedDependents.TryAdd(Environment.CurrentManagedThreadId, [existingDependent]));

        using var subject = new DependencyGraphBuilder(trackedDependents, dependent);

        var result = Assert.Single(trackedDependents);

        Assert.Equal(Environment.CurrentManagedThreadId, result.Key);
        Assert.Equivalent(new HashSet<IDependent>() { existingDependent, dependent }, result.Value);
    }

    [Fact]
    public void Dispose_Removes_First_Dependent() {
        var trackedDependents = new ConcurrentDictionary<int, HashSet<IDependent>>();
        var dependent = Substitute.For<IDependent>();

        using (var subject = new DependencyGraphBuilder(trackedDependents, dependent)) { }

        Assert.Empty(trackedDependents);
    }

    [Fact]
    public void Dispose_Removes_Second_Dependent() {
        var trackedDependents = new ConcurrentDictionary<int, HashSet<IDependent>>();
        var existingDependent = Substitute.For<IDependent>();
        var dependent = Substitute.For<IDependent>();

        Assert.True(trackedDependents.TryAdd(Environment.CurrentManagedThreadId, [existingDependent]));

        using (var subject = new DependencyGraphBuilder(trackedDependents, dependent)) { }
        
        Assert.Equivalent(new HashSet<IDependent>() { existingDependent }, Assert.Single(trackedDependents).Value);
    }
}
