using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class DependencyGraph2Tests {
    private class EffectCallTracker {
        public int Value { get; set; }
        public int Calls { get; private set; }
        

        public EffectCallTracker(StateProvider2 stateProvider, Func<int> valueProvider) {
            new Effect2(stateProvider, () => {
                Calls++;
                Value = valueProvider();
            });
        }
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Single_Dependency_Correctly() {
        var stateProvider = new StateProvider2();
        var mutableState = new MutableState2<int>(stateProvider, 41);
        var tracker = new EffectCallTracker(stateProvider, () => mutableState.Value);

        mutableState.Set(42);

        Assert.Equal(42, tracker.Value);
        Assert.Equal(2, tracker.Calls);
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Chain_Correctly() {
        var stateProvider = new StateProvider2();
        var mutableState = new MutableState2<int>(stateProvider, 41);
        var computedState = new ComputedState2<int>(stateProvider, () => mutableState.Value);
        var tracker = new EffectCallTracker(stateProvider, () => computedState.Value);

        mutableState.Set(42);

        Assert.Equal(42, tracker.Value);
        Assert.Equal(2, tracker.Calls);
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Deep_Linked_Chain_Correctly() {
        var stateProvider = new StateProvider2();
        var mutableState = new MutableState2<int>(stateProvider, 20);
        var computedState = new ComputedState2<int>(stateProvider, () => mutableState.Value);
        var trackers = new List<EffectCallTracker>() {
            new(stateProvider, () => mutableState.Value + computedState.Value),
            new(stateProvider, () => computedState.Value + mutableState.Value),
        };

        mutableState.Set(21);

        Assert.Multiple([.. trackers.Select<EffectCallTracker, Action>(tracker => () => {
            Assert.Equal(42, tracker.Value);
            Assert.Equal(2, tracker.Calls);
        })]);
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Diamond_Correctly() {
        var stateProvider = new StateProvider2();
        var mutableState = new MutableState2<int>(stateProvider, 20);
        var computedState1 = new ComputedState2<int>(stateProvider, () => mutableState.Value);
        var computedState2 = new ComputedState2<int>(stateProvider, () => mutableState.Value);
        var trackers = new List<EffectCallTracker>() {
            new(stateProvider, () => computedState1.Value + computedState2.Value),
            new(stateProvider, () => computedState2.Value + computedState1.Value),
        };
        
        mutableState.Set(21);

        Assert.Multiple([.. trackers.Select<EffectCallTracker, Action>(tracker => () => {
            Assert.Equal(42, tracker.Value);
            Assert.Equal(2, tracker.Calls);
        })]);
    }

    [Fact]
    public void EvaluateDependents_Evaluates_Deep_Linked_Diamond_Correctly() {
        var stateProvider = new StateProvider2();
        var mutableState = new MutableState2<int>(stateProvider, 13);
        var computedState1 = new ComputedState2<int>(stateProvider, () => mutableState.Value);
        var computedState2 = new ComputedState2<int>(stateProvider, () => mutableState.Value);

        var trackers = new List<EffectCallTracker>() {
            new(stateProvider, () => mutableState.Value + computedState1.Value + computedState2.Value),
            new(stateProvider, () => mutableState.Value + computedState2.Value + computedState1.Value),
            new(stateProvider, () => computedState1.Value + mutableState.Value + computedState2.Value),
            new(stateProvider, () => computedState2.Value + mutableState.Value + computedState1.Value),
            new(stateProvider, () => computedState1.Value + computedState2.Value + mutableState.Value),
            new(stateProvider, () => computedState2.Value + computedState1.Value + mutableState.Value),
        };

        mutableState.Set(14);

        Assert.Multiple([.. trackers.Select<EffectCallTracker, Action>(tracker => () => {
            Assert.Equal(42, tracker.Value);
            Assert.Equal(2, tracker.Calls);
        })]);
    }

    [Fact]
    public void EvaluateDependents_In_Transaction_Evaluates_Divergent_Chain_Correctly() {
        var stateProvider = new StateProvider2();
        var mutableState1 = new MutableState2<int>(stateProvider, 6);
        var mutableState2 = new MutableState2<int>(stateProvider, 6);
        var computedState = new ComputedState2<int>(stateProvider, () => mutableState1.Value + mutableState2.Value);

        var tracker = new EffectCallTracker(stateProvider, () => computedState.Value * 3);

        stateProvider.ExecuteTransaction(() => {
            mutableState1.Set(7);
            mutableState2.Set(7);
        });

        Assert.Equal(42, tracker.Value);
        Assert.Equal(2, tracker.Calls);
    }

    [Fact]
    public void EvaluateDependents_In_Transaction_Evaluates_Deep_Linked_Divergent_Chain_Correctly() {
        var stateProvider = new StateProvider2();
        var mutableState1 = new MutableState2<int>(stateProvider, 6);
        var mutableState2 = new MutableState2<int>(stateProvider, 6);
        var computedState = new ComputedState2<int>(stateProvider, () => mutableState1.Value + mutableState2.Value);

        var trackers = new List<EffectCallTracker>() {
            new(stateProvider, () => computedState.Value * 2 + mutableState1.Value + mutableState2.Value),
            new(stateProvider, () => computedState.Value * 2 + mutableState2.Value + mutableState1.Value),
            new(stateProvider, () => mutableState1.Value + computedState.Value * 2 + mutableState2.Value),
            new(stateProvider, () => mutableState2.Value + computedState.Value * 2 + mutableState1.Value),
            new(stateProvider, () => mutableState1.Value + mutableState2.Value + computedState.Value * 2),
            new(stateProvider, () => mutableState2.Value + mutableState1.Value + computedState.Value * 2),
        };

        stateProvider.ExecuteTransaction(() => {
            mutableState1.Set(7);
            mutableState2.Set(7);
        });

        Assert.Multiple([.. trackers.Select<EffectCallTracker, Action>(tracker => () => {
            Assert.Equal(42, tracker.Value);
            Assert.Equal(2, tracker.Calls);
        })]);
    }

    [Fact]
    public void EvaluateDependents_With_Conditional_Gives_ExpectedResults_Boolean_First() {
        var stateProvider = new StateProvider2();
        var mutableBoolean = new MutableState2<bool>(stateProvider, false);
        var mutableState = new MutableState2<int>(stateProvider, 41);

        var tracker = new EffectCallTracker(stateProvider, () => mutableBoolean.Value ? mutableState.Value : 0);

        mutableBoolean.Set(true);
        mutableState.Set(42);

        Assert.Equal(42, tracker.Value);
        Assert.Equal(3, tracker.Calls);
    }

    [Fact]
    public void EvaluateDependents_With_Conditional_Gives_ExpectedResults_Boolean_Last() {
        var stateProvider = new StateProvider2();
        var mutableBoolean = new MutableState2<bool>(stateProvider, false);
        var mutableState = new MutableState2<int>(stateProvider, 41);

        var tracker = new EffectCallTracker(stateProvider, () => mutableBoolean.Value ? mutableState.Value : 0);

        mutableState.Set(42);
        mutableBoolean.Set(true);

        Assert.Equal(42, tracker.Value);
        Assert.Equal(2, tracker.Calls);
    }

    [Fact]
    public void EvaluateDependents_With_Conditional_Gives_ExpectedResults_Inside_Transaction() {
        var stateProvider = new StateProvider2();
        var mutableBoolean = new MutableState2<bool>(stateProvider, false);
        var mutableState = new MutableState2<int>(stateProvider, 41);

        var tracker = new EffectCallTracker(stateProvider, () => mutableBoolean.Value ? mutableState.Value : 0);

        stateProvider.ExecuteTransaction(() => {
            mutableState.Set(42);
            mutableBoolean.Set(true);
        });
        
        Assert.Equal(42, tracker.Value);
        Assert.Equal(2, tracker.Calls);
    }
}
