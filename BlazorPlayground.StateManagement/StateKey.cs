using System;

namespace BlazorPlayground.StateManagement;

public record struct StateKey(Type Type, string? Name);
