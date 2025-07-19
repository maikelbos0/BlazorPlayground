using System;

namespace BlazorPlayground.StateManagement;

public class InvalidStateTypeException : Exception {
    public InvalidStateTypeException(StateKey key, Type expectedType, Type foundType) : base($"Expected type '{expectedType.FullName}' but found type '{foundType.FullName}' for key '{key}'") {
        Key = key;
        ExpectedType = expectedType;
        FoundType = foundType;
    }

    public StateKey Key { get; }
    public Type ExpectedType { get; }
    public Type FoundType { get; }
}
