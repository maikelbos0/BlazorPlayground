using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BlazorPlayground.StateManagement;

public class WeakReferenceEqualityComparer<T> : IEqualityComparer<WeakReference<T>> where T : class {
    public static WeakReferenceEqualityComparer<T> Instance { get; } = new();

    public bool Equals(WeakReference<T>? x, WeakReference<T>? y) {
        if (x == y) return true;

        T? targetX = null;
        T? targetY = null;

        _ = x?.TryGetTarget(out targetX);
        _ = y?.TryGetTarget(out targetY);

        return targetX == targetY;
    }

    public int GetHashCode([DisallowNull] WeakReference<T> obj) {
        if (obj.TryGetTarget(out var target)) {
            return target.GetHashCode();
        }

        return 0;
    }
}
