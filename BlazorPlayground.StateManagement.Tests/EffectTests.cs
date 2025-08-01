﻿using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class EffectTests {
    [Fact]
    public void Constructor() {
        var result = 0;
        var stateProvider = new StateProvider();
        var subject = new Effect(stateProvider, () => result = 42);

        Assert.Equal(42, result);
    }

    [Fact]
    public void Evaluate() {
        var value = 41;
        var result = 0;
        var stateProvider = new StateProvider();
        var subject = new Effect(stateProvider, () => result = value);

        value = 42;
        subject.Evaluate();

        Assert.Equal(42, result);
    }
}
