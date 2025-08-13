using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class DependencyGraphTests {
    private class ComputedStateCallTracker<T> {
        public int Calls { get; private set; }
        public ComputedState<T> State { get; }

        public ComputedStateCallTracker(StateProvider stateProvider, Func<T> computation) {
            State = new ComputedState<T>(stateProvider, () => {
                Calls++;
                return computation();
            });
        }
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Single_Dependency_Correctly() {
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 41);
        var tracker = new ComputedStateCallTracker<int>(stateProvider, () => mutableState.Value);

        mutableState.Set(42);

        Assert.Equal(42, tracker.State.Value);
        Assert.Equal(2, tracker.Calls);
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Chain_Correctly() {
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 41);
        var computedState = new ComputedState<int>(stateProvider, () => mutableState.Value);
        var tracker = new ComputedStateCallTracker<int>(stateProvider, () => computedState.Value);

        mutableState.Set(42);

        Assert.Equal(42, tracker.State.Value);
        Assert.Equal(2, tracker.Calls);
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Deep_Linked_Chain_Correctly() {
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 20);
        var computedState = new ComputedState<int>(stateProvider, () => mutableState.Value);
        var trackers = new List<ComputedStateCallTracker<int>>() {
            new(stateProvider, () => mutableState.Value + computedState.Value),
            new(stateProvider, () => computedState.Value + mutableState.Value),
        };

        mutableState.Set(21);

        Assert.Multiple([.. trackers.Select<ComputedStateCallTracker<int>, Action>(tracker => () => {
            Assert.Equal(42, tracker.State.Value);
            Assert.Equal(2, tracker.Calls);
        })]);
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Diamond_Correctly() {
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 20);
        var computedState1 = new ComputedState<int>(stateProvider, () => mutableState.Value);
        var computedState2 = new ComputedState<int>(stateProvider, () => mutableState.Value);
        var trackers = new List<ComputedStateCallTracker<int>>() {
            new(stateProvider, () => computedState1.Value + computedState2.Value),
            new(stateProvider, () => computedState2.Value + computedState1.Value),
        };

        mutableState.Set(21);

        Assert.Multiple([.. trackers.Select<ComputedStateCallTracker<int>, Action>(tracker => () => {
            Assert.Equal(42, tracker.State.Value);
            Assert.Equal(2, tracker.Calls);
        })]);
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Deep_Linked_Diamond_Correctly() {
        var stateProvider = new StateProvider();
        var mutableState = new MutableState<int>(stateProvider, 13);
        var computedState1 = new ComputedState<int>(stateProvider, () => mutableState.Value);
        var computedState2 = new ComputedState<int>(stateProvider, () => mutableState.Value);

        var trackers = new List<ComputedStateCallTracker<int>>() {
            new(stateProvider, () => mutableState.Value + computedState1.Value + computedState2.Value),
            new(stateProvider, () => mutableState.Value + computedState2.Value + computedState1.Value),
            new(stateProvider, () => computedState1.Value + mutableState.Value + computedState2.Value),
            new(stateProvider, () => computedState2.Value + mutableState.Value + computedState1.Value),
            new(stateProvider, () => computedState1.Value + computedState2.Value + mutableState.Value),
            new(stateProvider, () => computedState2.Value + computedState1.Value + mutableState.Value),
        };

        mutableState.Set(14);

        Assert.Multiple([.. trackers.Select<ComputedStateCallTracker<int>, Action>(tracker => () => {
            Assert.Equal(42, tracker.State.Value);
            Assert.Equal(2, tracker.Calls);
        })]);
    }
}
