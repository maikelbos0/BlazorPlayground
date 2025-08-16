using NSubstitute;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Xunit;

namespace BlazorPlayground.StateManagement.Tests;

public class StateProviderTests {
    [Fact]
    public void Mutable() {
        var subject = new StateProvider();

        var result = subject.Mutable(42);

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Mutable_With_IEqualityComparer() {
        var subject = new StateProvider();

        var result = subject.Mutable(42, Substitute.For<IEqualityComparer<int>>());

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Computed() {
        var subject = new StateProvider();
        var mutableState = subject.Mutable(42);

        var result = subject.Computed(() => mutableState.Value);

        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Effect() {
        var result = 41;
        var subject = new StateProvider();
        var mutableState = subject.Mutable(42);

        subject.Effect(() => result = mutableState.Value);

        Assert.Equal(42, result);
    }

    [Fact]
    public void TryRegisterForTransaction_Does_Nothing_Outside_Transaction() {
        var subject = new StateProvider();
        var transactionDependentsInfo = typeof(StateProvider).GetField("transactionDependents", BindingFlags.Instance | BindingFlags.NonPublic);
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();

        Assert.NotNull(transactionDependentsInfo);
        var transactionDependents = Assert.IsType<ThreadLocal<Dictionary<IDependent, nuint>>>(transactionDependentsInfo.GetValue(subject));

        Assert.False(subject.TryRegisterForTransaction(new Dictionary<IDependent, nuint>() {
            { dependent1, 2 },
            { dependent2, 5 },
        }));

        Assert.Null(transactionDependents.Value);
    }

    [Fact]
    public void TryRegisterForTransaction_Adds_Dependents_Inside_Transaction() {
        var subject = new StateProvider();
        var transactionDependentsInfo = typeof(StateProvider).GetField("transactionDependents", BindingFlags.Instance | BindingFlags.NonPublic);
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();

        Assert.NotNull(transactionDependentsInfo);
        var transactionDependents = Assert.IsType<ThreadLocal<Dictionary<IDependent, nuint>>>(transactionDependentsInfo.GetValue(subject));

        transactionDependents.Value = [];

        Assert.True(subject.TryRegisterForTransaction(new Dictionary<IDependent, nuint>() {
            { dependent1, 2 },
            { dependent2, 5 },
        }));

        Assert.NotNull(transactionDependents.Value);
        Assert.Equal(2u, Assert.Contains(dependent1, transactionDependents.Value));
        Assert.Equal(5u, Assert.Contains(dependent2, transactionDependents.Value));
    }

    [Fact]
    public void TryRegisterForTransaction_Updates_Dependents_Inside_Transaction() {
        var subject = new StateProvider();
        var transactionDependentsInfo = typeof(StateProvider).GetField("transactionDependents", BindingFlags.Instance | BindingFlags.NonPublic);
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();

        Assert.NotNull(transactionDependentsInfo);
        var transactionDependents = Assert.IsType<ThreadLocal<Dictionary<IDependent, nuint>>>(transactionDependentsInfo.GetValue(subject));

        transactionDependents.Value = new() {
            { dependent1, 1 },
            { dependent2, 4 }
        };

        Assert.True(subject.TryRegisterForTransaction(new Dictionary<IDependent, nuint>() {
            { dependent1, 2 },
            { dependent2, 5 },
        }));

        Assert.NotNull(transactionDependents.Value);
        Assert.Equal(3u, Assert.Contains(dependent1, transactionDependents.Value));
        Assert.Equal(9u, Assert.Contains(dependent2, transactionDependents.Value));
    }

    [Fact]
    public void ExecuteTransaction() {
        var subject = new StateProvider();
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();

        subject.ExecuteTransaction(() => {
            Assert.True(subject.TryRegisterForTransaction(new Dictionary<IDependent, nuint>() {
                { dependent1, 2 },
                { dependent2, 5 },
            }));
        });

        Received.InOrder(() => {
            dependent1.Evaluate();
            dependent2.Evaluate();
        });
    }

    [Fact]
    public void ExecuteTransaction_Nested_Only_Executes_Once() {
        var subject = new StateProvider();
        var dependent1 = Substitute.For<IDependent>();
        var dependent2 = Substitute.For<IDependent>();

        subject.ExecuteTransaction(() => {
            subject.ExecuteTransaction(() => {
                Assert.True(subject.TryRegisterForTransaction(new Dictionary<IDependent, nuint>() {
                    { dependent1, 2 },
                    { dependent2, 5 },
                }));
            });

            subject.ExecuteTransaction(() => {
                Assert.True(subject.TryRegisterForTransaction(new Dictionary<IDependent, nuint>() {
                    { dependent1, 2 },
                    { dependent2, 5 },
                }));
            });
        });

        dependent1.Received(1).Evaluate();
        dependent2.Received(1).Evaluate();
    }
}
